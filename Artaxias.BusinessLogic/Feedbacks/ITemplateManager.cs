using Artaxias.Web.Common.DataTransferObjects.Feadback;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Artaxias.BusinessLogic.Feedbacks
{
    public interface ITemplateManager : IManager<int, TemplateRequest, TemplateResponse>
    {
        Task<List<AnswerOptionTypeDetails>> GetAllAnswerOptionTypes();
    }
}
