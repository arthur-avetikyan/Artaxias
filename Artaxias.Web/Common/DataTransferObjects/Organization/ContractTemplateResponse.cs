using System;

namespace Artaxias.Web.Common.DataTransferObjects.Organization
{
    public class ContractTemplateResponse
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int CreatorId { get; set; }

        public string CreatorName { get; set; }

        public DateTime CreatedDatetimeUTC { get; set; }
    }
}
