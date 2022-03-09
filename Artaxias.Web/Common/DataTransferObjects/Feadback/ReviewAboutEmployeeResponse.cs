using Artaxias.Web.Common.DataTransferObjects.Organization;

using System;

namespace Artaxias.Web.Common.DataTransferObjects.Feadback
{
    public class ReviewAboutEmployeeResponse
    {
        public int ReviewId { get; set; }

        public string ReviewTitle { get; set; }

        public EmployeeInfo Reviewee { get; set; }

        public EmployeeInfo Reviewer { get; set; }

        public DateTime ProvidedDate { get; set; }
    }
}