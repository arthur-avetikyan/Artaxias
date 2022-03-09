using Artaxias.Web.Common.DataTransferObjects.Organization;

namespace Artaxias.Web.Common.DataTransferObjects.Feadback
{
    public class ProvideFeedbackResponse
    {
        public int Id { get; set; }

        public EmployeeInfo Reviewee { get; set; }

        public EmployeeInfo Reviewer { get; set; }

        public TemplateResponse Template { get; set; }
    }
}
