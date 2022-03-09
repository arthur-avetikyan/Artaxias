
using System.Collections.Generic;

namespace Artaxias.Web.Common.DataTransferObjects.Feadback
{
    public class AnswerResponse
    {
        public int Id { get; set; }

        public AnswerOptionTypeDetails OptionType { get; set; }

        public List<AnswerValueResponse> Values { get; set; }
    }
}