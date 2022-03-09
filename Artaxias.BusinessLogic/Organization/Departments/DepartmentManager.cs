using Artaxias.Data;
using Artaxias.Data.Models.Organization;
using Artaxias.Web.Common.DataTransferObjects.Organization;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artaxias.BusinessLogic.Organization.Departments
{
    public class DepartmentManager : IDepartmentManager
    {
        private readonly IRepository<Department> _departmentRepository;
        private readonly IRepository<EmployeeDepartment> _employeeDepartmentRepository;

        public DepartmentManager(IRepository<Department> departmentRepository, IRepository<EmployeeDepartment> employeeDepartmentRepository)
        {
            _departmentRepository = departmentRepository;
            _employeeDepartmentRepository = employeeDepartmentRepository;
        }

        public async Task<DepartmentResponse> CreateAsync(DepartmentRequest request)
        {
            if (_departmentRepository.Get().Any(r => r.Id == request.Id))
            {
                throw new ArgumentException("Department already exists");
            }

            Department newDepartment = new Department { Name = request.Name };

            foreach (int employeeId in request.Staff)
            {
                newDepartment.Staff.Add(new EmployeeDepartment { EmployeeId = employeeId });
            }

            _departmentRepository.Insert(newDepartment);
            await _departmentRepository.SaveChangesAsync();
            return newDepartment.Map();
        }

        public async Task DeleteAsync(int key)
        {
            Department department = await _departmentRepository.Get(d => d.Id == key).FirstOrDefaultAsync();
            if (department == null)
            {
                throw new ArgumentException("Department not found");
            }

            _departmentRepository.Delete(department);
            await _departmentRepository.SaveChangesAsync();
        }

        public async Task<DepartmentResponse> GetAsync(int key)
        {
            return await _departmentRepository.Get(d => d.Id == key)
                                              .Map()
                                              .FirstOrDefaultAsync();
        }

        public async Task<List<DepartmentResponse>> GetListAsync(int pageSize = 10, int currentPage = 0)
        {
            return await _departmentRepository.Get()
                                              .Skip(currentPage * pageSize)
                                              .Take(pageSize)
                                              .OrderBy(d => d.Id)
                                              .Map()
                                              .ToListAsync();
        }

        public async Task<DepartmentResponse> UpdateAsync(int key, DepartmentRequest request)
        {
            Department department = await _departmentRepository.Get(d => d.Id == key)
                                                               .Include(d => d.Staff)
                                                               .FirstOrDefaultAsync();
            if (department == null || string.IsNullOrWhiteSpace(request.Name))
            {
                throw new ArgumentException("Department does not exist");
            }

            department.Name = request.Name;

            IEnumerable<int> newEmployees = request.Staff.Where(employeeId => !department.Staff.Any(s => s.EmployeeId == employeeId));
            foreach (int employeeId in newEmployees)
            {
                department.Staff.Add(new EmployeeDepartment { EmployeeId = employeeId });
            }

            IEnumerable<EmployeeDepartment> formerEmployees = department.Staff.Where(emp => !request.Staff.Contains(emp.EmployeeId));
            foreach (EmployeeDepartment item in formerEmployees)
            {
                _employeeDepartmentRepository.Delete(item);
            }

            _departmentRepository.Update(department);
            await _departmentRepository.SaveChangesAsync();
            await _employeeDepartmentRepository.SaveChangesAsync();

            return department.Map();
        }
    }
}
