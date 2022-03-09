
using Artaxias.Data;
using Artaxias.Data.Models.Organization;

using System.Collections.Generic;

namespace Artaxias.BusinessLogic.Organization.Absences
{
    public class AbsenceStateVerifier : IDomainStateVerifier<Absence>
    {
        private readonly List<IDomainStateVerifyingRule<Absence>> _rules = new List<IDomainStateVerifyingRule<Absence>>();

        private readonly IRepository<Absence> _absenceRepository;


        public AbsenceStateVerifier(IRepository<Absence> absenceRepository)
        {
            _absenceRepository = absenceRepository;
            _rules.Add(new AbsenceDomainStateCompletedRule());
        }

        public void Verify(Absence entity)
        {
            foreach (IDomainStateVerifyingRule<Absence> rule in _rules)
            {
                rule.CheckAndApply(entity);
            }

            _absenceRepository.SaveChangesAsync();
        }

        public void Verify(IEnumerable<Absence> entities)
        {
            foreach (Absence entity in entities)
            {
                foreach (IDomainStateVerifyingRule<Absence> rule in _rules)
                {
                    rule.CheckAndApply(entity);
                }
            }

            _absenceRepository.SaveChangesAsync();
        }
    }
}
