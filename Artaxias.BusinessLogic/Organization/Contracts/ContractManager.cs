using Artaxias.BusinessLogic.FileManagement;
using Artaxias.Core.Configurations;
using Artaxias.Data;
using Artaxias.Data.Models.Organization;
using Artaxias.Document.Generators;
using Artaxias.Document.Processors;
using Artaxias.Models;
using Artaxias.Web.Common.DataTransferObjects.File;
using Artaxias.Web.Common.DataTransferObjects.Organization;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Artaxias.BusinessLogic.Organization.Contracts
{
    public class ContractManager : IContractManager
    {
        private readonly IRepository<ContractTemplate> _repository;
        private readonly IRepository<Employee> _employeeRepository;
        private readonly ITemplateProcessor _templateProcessor;
        private readonly IDocumentGenerator _documentGenerator;
        private readonly IFileManager _fileManager;
        private readonly ApplicationConfiguration _applicationConfiguration;

        public ContractManager(ITemplateProcessor templateProcessor,
                               IOptions<ApplicationConfiguration> applicationConfiguration,
                               IRepository<ContractTemplate> repository,
                               IFileManager fileManager,
                               IDocumentGenerator documentGenerator,
                               IRepository<Employee> employeeRepository)
        {
            _templateProcessor = templateProcessor;
            _applicationConfiguration = applicationConfiguration.Value;
            _repository = repository;
            _fileManager = fileManager;
            _documentGenerator = documentGenerator;
            _employeeRepository = employeeRepository;
        }

        public async Task<ContractMappings> SaveAsync(ContractTemplateRequest request)
        {
            if (_repository.Get().Any(ct => ct.Title.Equals(request.Title)))
            {
                throw new ArgumentException("Contract Template with the same title already exists.");
            }

            ContractTemplate template = request.Map(new ContractTemplate());
            template.Path = await _fileManager.SaveFileAsync(request.Document, _applicationConfiguration.DocumentTemplatesFilePath);

            if (string.IsNullOrWhiteSpace(template.Path))
            {
                throw new ArgumentException("File is corrupted or empty");
            }

            IList<string> mappings = _templateProcessor.ProcessTemplate(Path.Combine(Directory.GetCurrentDirectory(), template.Path));
            foreach (string item in mappings)
            {
                template.Mappings.Add(new ContractTemplateMapping
                {
                    TemplateField = item
                });
            }
            _repository.Insert(template);
            await _repository.SaveChangesAsync();


            ContractMappings contractMappings = new ContractMappings
            {
                ContractTemplateId = template.Id,
                Title = template.Title,
                Mappings = new Dictionary<string, string>()
            };

            foreach (string item in mappings)
            {
                contractMappings.Mappings.Add(item, string.Empty);
            }
            return contractMappings;
        }

        public async Task DeleteAsync(int key)
        {
            ContractTemplate entity = await _repository.Get(ct => ct.Id == key).FirstOrDefaultAsync();
            if (entity == null)
            {
                throw new ArgumentException("Contract Template was not found.");
            }

            _fileManager.DeleteFile(Path.Combine(Directory.GetCurrentDirectory(), entity.Path));
            _repository.Delete(entity);
            await _repository.SaveChangesAsync();
        }

        public async Task<ContractTemplateResponse> GetAsync(int key)
        {
            return await _repository.Get(item => item.Id.Equals(key)).Map().FirstOrDefaultAsync();
        }

        public async Task<List<ContractTemplateResponse>> GetListAsync(int pageSize = 10, int currentPage = 0)
        {
            return await _repository.Get()
                                    .OrderByDescending(x => x.CreatedDatetimeUTC)
                                    .Skip(currentPage * pageSize)
                                    .Take(pageSize)
                                    .Map()
                                    .ToListAsync();
        }

        public IEnumerable<string> GetAvailablePropertiesAsync()
        {
            HashSet<string> propertyNames = new HashSet<string>();
            GetPropertyNames(propertyNames, typeof(Employee).GetProperties());
            return propertyNames;
        }

        private void GetPropertyNames(HashSet<string> names, IEnumerable<PropertyInfo> properties)
        {
            foreach (PropertyInfo property in properties)
            {
                if (property.PropertyType.IsEnum || property.Name.Contains("ID", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
                if (property.PropertyType.IsValueType)
                {
                    names.Add($"{property.DeclaringType.Name}.{property.Name}");
                }
                if (property.PropertyType.Equals(typeof(string)))
                {
                    names.Add($"{property.DeclaringType.Name}.{ property.Name}");
                }
                if (property.PropertyType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
                {
                    GetPropertyNames(names, property.PropertyType.GetGenericArguments().FirstOrDefault().GetProperties());
                }
                if (property.PropertyType.IsClass && property.PropertyType == typeof(User))
                {
                    names.Add($"{property.PropertyType.Name}.{property.PropertyType.GetProperty("FirstName").Name}");
                    names.Add($"{property.PropertyType.Name}.{property.PropertyType.GetProperty("LastName").Name}");
                }
            }
        }

        public async Task<ContractMappings> GetContractMappingsAsync(int contractTemplateId)
        {
            ContractTemplate mappings = await _repository.Get(m => m.Id == contractTemplateId)
                                            .Include(m => m.Mappings)
                                            .FirstOrDefaultAsync();
            ContractMappings result = new ContractMappings
            {
                ContractTemplateId = mappings.Id,
                Title = mappings.Title
            };
            foreach (ContractTemplateMapping item in mappings.Mappings)
            {
                result.Mappings.Add(item.TemplateField, item.EntityField);
            }
            return result;
        }

        public async Task<FileDto> GenerateAsync(ContractGenerationRequest request)
        {
            ContractTemplate contract = await _repository.Get(c => c.Id == request.ContractTemplateId)
                 .Include(c => c.Mappings).FirstOrDefaultAsync();

            Employee employee = await _employeeRepository.Get(e => e.Id == request.EmployeeId)
                     .Include(e => e.User)
                     // .Include(e => e.Department)
                     .Include(e => e.Salaries)
                     .FirstOrDefaultAsync();

            _ = employee.Salaries.OrderByDescending(s => s.AssignmentDate);

            Dictionary<string, string> mappings = new();

            foreach (ContractTemplateMapping mapping in contract.Mappings)
            {
                string value = GetValues(mapping.EntityField, employee);
                mappings.Add(mapping.TemplateField, value);
            }

            string absolutePath = Path.Combine(Directory.GetCurrentDirectory(), contract.Path);
            string filePath = _documentGenerator.GenerateDocumnet(absolutePath, mappings);

            FileDto file = await _fileManager.GetFileAsync(filePath, $"{contract.Title}_{Guid.NewGuid()}.docx");
            _fileManager.DeleteFile(filePath);
            return file;
        }

        private string GetValues(string searchTerm, Employee employee)
        {
            Type type = employee.GetType();
            string typeName = searchTerm.Substring(0, searchTerm.IndexOf('.'));
            string propertyName = searchTerm[(searchTerm.IndexOf('.') + 1)..];

            if (type.Name.Equals(typeName))
            {
                PropertyInfo property = type.GetProperty(propertyName);
                return property.GetValue(employee).ToString();
            }
            PropertyInfo complexProperty = type.GetProperties().FirstOrDefault(p => p.PropertyType.Name.Equals(typeName));

            if (complexProperty == null)
            {
                complexProperty = type.GetProperties()
                    .Where(p => p.PropertyType.IsGenericType)
                    .Where(p => p.PropertyType.GetGenericArguments().FirstOrDefault(arg => arg.Name.Equals(typeName)) != null)
                    .FirstOrDefault();

                IEnumerable complexPropertyValue = (IEnumerable)complexProperty.GetValue(employee);
                foreach (object item in complexPropertyValue)
                {
                    PropertyInfo requiredFirstProperty = item.GetType().GetProperty(propertyName);
                    return requiredFirstProperty.GetValue(item).ToString();
                }
            }

            PropertyInfo requiredProperty = complexProperty.PropertyType.GetProperty(propertyName);
            return requiredProperty.GetValue(complexProperty.GetValue(employee)).ToString();
        }

        public async Task<string> SaveMappingsAsync(ContractMappings request)
        {
            ContractTemplate mappings = await _repository.Get(m => m.Id == request.ContractTemplateId)
                                            .Include(m => m.Mappings)
                                            .FirstOrDefaultAsync();
            if (mappings == null)
            {
                throw new ArgumentException("Contract Template was not found.");
            }

            foreach (KeyValuePair<string, string> item in request.Mappings)
            {
                ContractTemplateMapping mapping = mappings.Mappings.FirstOrDefault(m => m.TemplateField.Equals(item.Key, StringComparison.OrdinalIgnoreCase));
                if (mapping != null)
                {
                    mapping.EntityField = item.Value;
                }
            }
            _repository.Update(mappings);
            await _repository.SaveChangesAsync();
            return "Succes";
        }



        public Task<ContractTemplateResponse> CreateAsync(ContractTemplateRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ContractTemplateResponse> UpdateAsync(int key, ContractTemplateRequest request)
        {
            throw new NotImplementedException();
        }
    }
}