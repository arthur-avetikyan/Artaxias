using Artaxias.Web.Common.DataTransferObjects.File;
using Artaxias.Web.Common.DataTransferObjects.Organization;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Artaxias.Web.Client.Services.Interfaces
{
    public interface IContractApiAccessor
    {
        Task DeleteTemplateAsync(int id);

        Task<ContractMappings> GetContractTemplatePropertiesAsync(int contractTemplateId);

        Task<List<ContractTemplateResponse>> GetTemplatesListAsync(int pageSize = 10, int currentPage = 0);

        Task<FileDto> GenerateContractAsync(ContractGenerationRequest request);

        Task<IEnumerable<string>> GetAvailablePropertiesOfEmployeeAsync();

        Task<string> CreateTemplateAsync(ContractTemplateRequest request);

        Task<ContractTemplateResponse> GetTemplateAsync(int id);

        Task<string> SaveContractMappings(ContractMappings request);
    }
}