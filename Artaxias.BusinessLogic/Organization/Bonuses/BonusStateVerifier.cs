using Artaxias.Data;
using Artaxias.Data.Models.Organization;
using System.Collections.Generic;



namespace Artaxias.BusinessLogic.Organization.Bonuses
{
    public class BonusStateVerifier : IDomainStateVerifier<Bonus>
    {
        private readonly List<IDomainStateVerifyingRule<Bonus>> _rules = new List<IDomainStateVerifyingRule<Bonus>>();
        private readonly IRepository<Bonus> _repository;

        public BonusStateVerifier(IRepository<Bonus> repository)
        {
            _repository = repository;
            _rules.Add(new BonusDomainStateCompletedRule());
        }

        public void Verify(Bonus entity)
        {
            foreach (IDomainStateVerifyingRule<Bonus> rule in _rules)
            {
                rule.CheckAndApply(entity);
            }
            _repository.SaveChangesAsync();
        }

        public void Verify(IEnumerable<Bonus> entities)
        {
            foreach (Bonus entity in entities)
            {
                foreach (IDomainStateVerifyingRule<Bonus> rule in _rules)
                {
                    rule.CheckAndApply(entity);
                }
            }
            _repository.SaveChangesAsync();
        }
    }
}
