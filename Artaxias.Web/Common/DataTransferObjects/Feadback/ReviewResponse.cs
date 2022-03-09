
using Artaxias.Web.Common.DataTransferObjects.Organization;

using System;
using System.Collections.Generic;

namespace Artaxias.Web.Common.DataTransferObjects.Feadback
{
    public class ReviewResponse
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int StateId { get; set; }

        public List<DepartmentInfo> Departments { get; set; }

        public TemplateResponse Template { get; set; }

        public DateTime Created { get; set; }

        public DateTime Deadline { get; set; }

        public List<ReviewerRevieweeInfo> ReviewerReviewees { get; set; }

        public bool DisplayMore { get; set; } = false;
    }
}
