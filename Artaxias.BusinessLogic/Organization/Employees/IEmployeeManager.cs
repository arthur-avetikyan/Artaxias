using Artaxias.Web.Common.DataTransferObjects.Organization;

using System.Threading.Tasks;

namespace Artaxias.BusinessLogic.Organization.Employees
{
    public interface IEmployeeManager : IManager<int, EmployeeRequest, EmployeeResponse>
    {
        Task<EmployeeResponse> EndContract(EndContractRequest details);
    }
}
