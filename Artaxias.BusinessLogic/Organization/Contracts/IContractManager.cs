using Artaxias.Web.Common.DataTransferObjects.File;
using Artaxias.Web.Common.DataTransferObjects.Organization;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Artaxias.BusinessLogic.Organization.Contracts
{
    public interface IContractManager : IManager<int, ContractTemplateRequest, ContractTemplateResponse>
    {
        Task<ContractMappings> SaveAsync(ContractTemplateRequest request);
        IEnumerable<string> GetAvailablePropertiesAsync();
        Task<ContractMappings> GetContractMappingsAsync(int contractTemplateId);
        Task<FileDto> GenerateAsync(ContractGenerationRequest request);
        Task<string> SaveMappingsAsync(ContractMappings request);
    }
}