
using Artaxias.Data.Interfaces;
using System.Collections.Generic;

namespace Artaxias.BusinessLogic
{
    public interface IDomainStateVerifier<in T> where T : IDomainStateHolder
    {
        public void Verify(T entity);

        public void Verify(IEnumerable<T> entities);
    }
}
