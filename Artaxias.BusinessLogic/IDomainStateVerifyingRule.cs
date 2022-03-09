
using Artaxias.Data.Interfaces;

namespace Artaxias.BusinessLogic
{
    public interface IDomainStateVerifyingRule<in T> where T : IDomainStateHolder
    {
        public void CheckAndApply(T entity);
    }
}
