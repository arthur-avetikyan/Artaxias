
using Artaxias.Core.Enums;

namespace Artaxias.Data.Interfaces
{
    public interface IDomainStateHolder
    {
        public EDomainState EDomainState { get; }
    }
}
