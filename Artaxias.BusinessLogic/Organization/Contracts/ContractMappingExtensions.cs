using Artaxias.Data.Models.Organization;
using Artaxias.Web.Common.DataTransferObjects.Organization;

using System;
using System.Linq;

namespace Artaxias.BusinessLogic.Organization.Contracts
{
    public static class ContractMappingExtensions
    {
        public static IQueryable<ContractTemplateResponse> Map(this IQueryable<ContractTemplate> entity)
        {
            return entity.Select(ct => new ContractTemplateResponse
            {
                Id = ct.Id,
                Title = ct.Title,
                CreatedDatetimeUTC = ct.CreatedDatetimeUTC,
                CreatorId = ct.CreatedByUserId,
                CreatorName = $"{ct.CreatedByUser.FirstName} {ct.CreatedByUser.LastName}"
            });
        }

        public static ContractTemplate Map(this ContractTemplateRequest request, ContractTemplate entity)
        {
            entity.Title = request.Title;
            entity.CreatedByUserId = request.CreatorId;
            entity.CreatedDatetimeUTC = DateTime.UtcNow;
            return entity;
        }

        public static ContractTemplateResponse Map(this ContractTemplate entity)
        {
            return new ContractTemplateResponse
            {
                Id = entity.Id,
                Title = entity.Title,
                CreatorId = entity.CreatedByUserId,
                CreatorName = $"{entity.CreatedByUser.FirstName} {entity.CreatedByUser.LastName}",
                CreatedDatetimeUTC = entity.CreatedDatetimeUTC
            };
        }
    }
}
