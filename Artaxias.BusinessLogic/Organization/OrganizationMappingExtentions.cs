using Artaxias.Data.Models.Organization;
using Artaxias.Web.Common.DataTransferObjects.Organization;

using System.Linq;

namespace Artaxias.BusinessLogic.Organization
{
    internal static class OrganizationMappingExtentions
    {
        internal static DepartmentResponse Map(this Department entity)
        {
            return new DepartmentResponse
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }

        internal static IQueryable<DepartmentResponse> Map(this IQueryable<Department> entity)
        {
            return entity.Select(s => new DepartmentResponse
            {
                Id = s.Id,
                Name = s.Name,
                Staff = s.Staff.Select(employeeDepartment => new EmployeeInfo
                {
                    Id = employeeDepartment.EmployeeId,
                    FirstName = employeeDepartment.Employee.User.FirstName,
                    LastName = employeeDepartment.Employee.User.LastName,
                }).ToList()
            });
        }

        internal static IQueryable<SalaryResponse> Map(this IQueryable<Salary> entity)
        {
            return entity.Select(s => new SalaryResponse
            {
                Id = s.Id,
                AssignmentDate = s.AssignmentDate,
                GrossAmount = s.GrossAmount,
                NetAmount = s.NetAmount
            });
        }

        internal static SalaryResponse Map(this Salary entity)
        {
            return new SalaryResponse
            {
                Id = entity.Id,
                AssignmentDate = entity.AssignmentDate,
                GrossAmount = entity.GrossAmount,
                NetAmount = entity.NetAmount
            };
        }

        internal static IQueryable<EmployeeResponse> Map(this IQueryable<Employee> entity)
        {
            return entity.Select(e => new EmployeeResponse
            {
                Id = e.Id,
                FirstName = e.User.FirstName,
                LastName = e.User.LastName,
                UserName = e.User.UserName,
                Position = e.Position,
                ContractStart = e.ContractStart,
                ContractEnd = e.ContractEnd,
                Departments = e.Departments.Select(ed => new DepartmentInfo
                {
                    DepartmentId = ed.DepartmentId,
                    DepartmentName = ed.Department.Name
                }).ToList()
            });
        }

        internal static EmployeeResponse Map(this Employee entity)
        {
            return new EmployeeResponse
            {
                Id = entity.Id,
                Position = entity.Position,
                ContractStart = entity.ContractStart,
                ContractEnd = entity.ContractEnd,
                FirstName = entity.User.FirstName,
                LastName = entity.User.LastName,
                UserName = entity.User.UserName
            };
        }
    }
}
