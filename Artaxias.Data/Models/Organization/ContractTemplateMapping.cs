namespace Artaxias.Data.Models.Organization
{
    public partial class ContractTemplateMapping
    {
        public int Id { get; set; }

        public string TemplateField { get; set; }

        public string EntityField { get; set; }

        public int ContractTemplateId { get; set; }

        public ContractTemplate ContractTemplate { get; set; }
    }
}
