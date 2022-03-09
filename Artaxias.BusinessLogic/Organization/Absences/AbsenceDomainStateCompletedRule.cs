using Artaxias.Core.Enums;
using Artaxias.Data.Models.Organization;

using System;

namespace Artaxias.BusinessLogic.Organization.Absences
{
    public class AbsenceDomainStateCompletedRule : IDomainStateVerifyingRule<Absence>
    {
        public void CheckAndApply(Absence entity)
        {
            if (entity.EndDate >= DateTime.UtcNow)
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
