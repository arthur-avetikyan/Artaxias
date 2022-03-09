using Artaxias.Web.Common.DataTransferObjects.Organization;

using System.ComponentModel.DataAnnotations;

namespace Artaxias.Web.Common.DataTransferObjects.Feadback
{
    public class ReviewerRevieweeInfo
    {
        public ReviewerRevieweeInfo()
        {
            Reviewee = new EmployeeInfo();
            Reviewer = new EmployeeInfo();
        }

        [ValidateComplexType]
        public EmployeeInfo Reviewer { get; set; }

        [ValidateComplexType]
        public EmployeeInfo Reviewee { get; set; }

        public int StateId { get; set; }
    }
}