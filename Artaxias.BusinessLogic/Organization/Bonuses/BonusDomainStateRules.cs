using Artaxias.Core.Enums;
using Artaxias.Data.Models.Organization;

using System;


namespace Artaxias.BusinessLogic.Organization.Bonuses
{
    public class BonusDomainStateCompletedRule : IDomainStateVerifyingRule<Bonus>
    {
        public void CheckAndApply(Bonus entity)
        {
            if (entity.PaymentDate >= DateTime.UtcNow)
            {
                return;
            }

            if (entity.DomainStateId == (int)EDomainState.Approved)
            {
                entity.DomainStateId = (int)EDomainState.Completed;
            }
            else
            {
                entity.DomainStateId = (int)EDomainState.Abandoned;
            }
        }
    }
}
