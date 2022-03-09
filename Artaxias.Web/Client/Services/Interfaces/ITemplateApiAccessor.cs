using Artaxias.Web.Common.DataTransferObjects.Feadback;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Artaxias.Web.Client.Services.Interfaces
{
    public interface ITemplateApiAccessor
    {
        Task<string> CreateFeadbackTemplateAsync(TemplateRequest request);

        Task DeleteTemplateAsync(int id);

        Task<TemplateResponse> GetFeadbackTemplateAsync(int id);

        Task<List<TemplateResponse>> GetFeadbackTemplatesAsync(int pageSize = 10, int currentPage = 0);

        Task<TemplateResponse> UpdateFeadBackTemplateAsync(TemplateRequest request);

        Task<List<AnswerOptionTypeDetails>> GetAnswerOptionTypes();
    }
}
