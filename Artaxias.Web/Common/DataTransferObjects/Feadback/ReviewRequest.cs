using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Artaxias.Web.Common.DataTransferObjects.Feadback
{
    public class ReviewRequest
    {
        public ReviewRequest()
        {
            ReviewerReviewees = new List<ReviewerRevieweeInfo> { new ReviewerRevieweeInfo() };
            DepartmentIds = new List<int>();
        }

        [Required]
        [StringLength(1000, MinimumLength = 1, ErrorMessage = "Title is not long enough.")]
        public string Title { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Template is not selected")]
        public int TemplateId { get; set; }

        [Required]
        public List<int> DepartmentIds { get; set; }

        [DisplayName("Deadline")]
        [DataType(DataType.Date)]
        [Range(typeof(DateTime), "1/1/2000", "1/1/2100", ErrorMessage = "Date input is invalid")]
        public DateTime Deadline { get; set; }

        [ValidateComplexType]
        public List<ReviewerRevieweeInfo> ReviewerReviewees { get; set; }
    }
}
