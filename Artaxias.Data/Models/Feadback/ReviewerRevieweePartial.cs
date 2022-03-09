
using Artaxias.Core.Enums;
using Artaxias.Data.Interfaces;
using System;

namespace Artaxias.Data.Models.Feadback
{
    public partial class ReviewerReviewee : IDomainStateHolder
    {
        public EDomainState EDomainState
        {
            get
            {
                if (Enum.IsDefined(typeof(EDomainState), DomainStateId))
                {
                    return (EDomainState)DomainStateId;
                }
                {
                    throw new ArgumentOutOfRangeException(nameof(DomainStateId));
                }
            }
        }
    }
}
