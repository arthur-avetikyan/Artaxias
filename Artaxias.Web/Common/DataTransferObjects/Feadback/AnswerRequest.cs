
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Artaxias.Web.Common.DataTransferObjects.Feadback
{
    public class AnswerRequest
    {
        [Range(0, int.MaxValue)]
        public int Id { get; set; }

        [ValidateComplexType]
        public AnswerOptionTypeDetails OptionType { get; set; }

        [ValidateComplexType]
        public List<AnswerValueRequest> Values { get; set; }
    }
}