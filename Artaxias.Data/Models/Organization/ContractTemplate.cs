using Artaxias.Models;

using System;
using System.Collections.Generic;

namespace Artaxias.Data.Models.Organization
{
    public partial class ContractTemplate
    {
        public ContractTemplate()
        {
            Mappings = new List<ContractTemplateMapping>();
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Path { get; set; }

        public int CreatedByUserId { get; set; }

        public virtual User CreatedByUser { get; set; }

        public virtual DateTime CreatedDatetimeUTC { get; set; }

        public virtual DateTime? UpdatedDatetimeUTC { get; set; }

        public IList<ContractTemplateMapping> Mappings { get; set; }
    }
}
