
using System.Collections.Generic;

namespace Artaxias.Web.Common.DataTransferObjects.Feadback
{
    public class TemplateResponse
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int CreatedByUserId { get; set; }

        public string CreatorName { get; set; }

        public List<QuestionResponse> Questions { get; set; }

        public bool InUse { get; set; }
    }
}
