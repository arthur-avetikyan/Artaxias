
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Artaxias.BusinessLogic
{
    public interface IManager<in TKey, in TRequest, TResponse>
        where TResponse : class
        where TRequest : class
    {
        Task<TResponse> GetAsync(TKey key);

        Task<List<TResponse>> GetListAsync(int pageSize = 10, int currentPage = 0);

        Task<TResponse> CreateAsync(TRequest request);

        Task<TResponse> UpdateAsync(TKey key, TRequest request);

        Task DeleteAsync(TKey key);
    }
}
