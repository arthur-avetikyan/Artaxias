using Artaxias.Data;
using Artaxias.Data.Models.Organization;
using Artaxias.Models;
using Artaxias.Web.Common.DataTransferObjects.Organization;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artaxias.BusinessLogic.Organization.Employees
{
    public class EmployeeManager : IEmployeeManager
    {
        private readonly IRepository<Employee> _employeeRepository;
        private readonly IRepository<EmployeeDepartment> _employeeDepartmentRepository;
        private readonly IRepository<User> _userRepository;

        public EmployeeManager(IRepository<Employee> employeeRepository,
                               IRepository<User> userRepository,
                               IRepository<EmployeeDepartment> employeeDepartmentRepository)
        {
            _employeeRepository = employeeRepository;
            _userRepository = userRepository;
            _employeeDepartmentRepository = employeeDepartmentRepository;
        }

        public async Task<EmployeeResponse> GetAsync(int key)
        {
            EmployeeResponse employee = await _employeeRepository.Get(e => e.Id == key)
                .Map()
                .FirstOrDefaultAsync();
            return employee;
        }

        public async Task<List<EmployeeResponse>> GetListAsync(int pageSize = 10, int currentPage = 0)
        {
            List<EmployeeResponse> employees = await _employeeRepository.Get()
                .Skip(currentPage * pageSize)
                .Take(pageSize)
                .Map()
                .ToListAsync();
            return employees;
        }

        public async Task<EmployeeResponse> CreateAsync(EmployeeRequest employeeRequest)
        {
            //FIX EmployeeId used as UserId
            User user = await _userRepository.Get(u => u.Id == employeeRequest.Id).FirstOrDefaultAsync();
            if (user == null)
            {
                throw new ArgumentException("Employee was not found");
            }

            user.Employee = new Employee
            {
                Position = employeeRequest.Position,
                ContractStart = employeeRequest.ContractStart
            };

            foreach (int item in employeeRequest.DepartmentIds)
            {
                user.Employee.Departments.Add(new EmployeeDepartment { DepartmentId = item });
            }

            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();

            return user.Employee.Map();
        }

        public async Task<EmployeeResponse> UpdateAsync(int key, EmployeeRequest request)
        {
            Employee employee = await _employeeRepository.Get(e => e.Id == key)
                                                         .Include(e => e.User)
                                                         .Include(e => e.Departments)
                                                         .FirstOrDefaultAsync();
            if (employee == null)
            {
                throw new ArgumentException("Employee not found");
            }

            employee.Position = request.Position;
            employee.ContractStart = employee.ContractStart;
            employee.ContractEnd = null;
            employee.ContractStart = request.ContractStart;

            IEnumerable<int> newDepartments = request.DepartmentIds.Where(departmentId => !employee.Departments.Any(s => s.DepartmentId == departmentId));
            IEnumerable<EmployeeDepartment> formerDepartments = employee.Departments.Where(emp => !request.DepartmentIds.Contains(emp.DepartmentId));
            foreach (int id in newDepartments)
            {
                employee.Departments.Add(new EmployeeDepartment { DepartmentId = id });
            }

            foreach (EmployeeDepartment item in formerDepartments)
            {
                _employeeDepartmentRepository.Delete(item);
            }

            _employeeRepository.Update(employee);
            await _employeeRepository.SaveChangesAsync();
            await _employeeDepartmentRepository.SaveChangesAsync();
            return employee.Map();
        }

        public async Task DeleteAsync(int key)
        {
            Employee employee = await _employeeRepository.Get(e => e.Id == key).FirstOrDefaultAsync();
            if (employee == null)
            {
                throw new ArgumentException("Employee was not found");
            }

            _employeeRepository.Delete(employee);
            await _employeeRepository.SaveChangesAsync();
        }

        public async Task<EmployeeResponse> EndContract(EndContractRequest details)
        {
            Employee employee = await _employeeRepository.Get(e => e.Id == details.EmployeeId).Include(e => e.Departments).FirstOrDefaultAsync();
            if (employee == null)
            {
                throw new ArgumentException("Employee not found");
            }

            employee.ContractEnd = details.ContractEndDate;

            foreach (EmployeeDepartment item in employee.Departments)
            {
                _employeeDepartmentRepository.Delete(item);
            }
            await _employeeDepartmentRepository.SaveChangesAsync();

            _employeeRepository.Update(employee);
            await _employeeRepository.SaveChangesAsync();
            return employee.Map();
        }
    }
}
