using Artaxias.Data;
using Artaxias.Data.Models.Organization;
using Artaxias.Web.Common.DataTransferObjects.Organization;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artaxias.BusinessLogic.Organization.Salaries
{
    public class SalaryManager : ISalaryManager
    {
        private readonly IRepository<Salary> _salaryRepository;
        private readonly IRepository<Employee> _employeeRepository;

        public SalaryManager(IRepository<Salary> salarytRepository, IRepository<Employee> employeeRepository)
        {
            _salaryRepository = salarytRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task<List<SalaryResponse>> GetListAsync(int employeeId, int pageSize = 10, int currentPage = 0)
        {
            List<SalaryResponse> salaries = await _salaryRepository.Get(s => s.EmployeeId == employeeId)
                .OrderByDescending(s => s.AssignmentDate)
                .Map()
                .ToListAsync();
            return salaries;
        }

        public async Task<SalaryResponse> CreateAsync(SalaryRequest request)
        {
            Employee employee = await _employeeRepository.Get(e => e.Id == request.EmployeeId).Include(e => e.Salaries).FirstOrDefaultAsync();
            if (employee == null)
            {
                throw new ArgumentException("Employee not found");
            }

            Salary salary = new Salary
            {
                AssignmentDate = request.AssignmentDate,
                GrossAmount = request.GrossSalary,
                NetAmount = request.NetSalary
            };

            employee.Salaries.Add(salary);
            _employeeRepository.Update(employee);
            await _employeeRepository.SaveChangesAsync();

            return salary.Map();
        }

        public Task<SalaryResponse> GetAsync(int key)
        {
            throw new NotImplementedException();
        }

        public Task<List<SalaryResponse>> GetListAsync(int pageSize = 10, int currentPage = 0)
        {
            throw new NotImplementedException();
        }

        public Task<SalaryResponse> UpdateAsync(int key, SalaryRequest request)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int key)
        {
            throw new NotImplementedException();
        }
    }
}
