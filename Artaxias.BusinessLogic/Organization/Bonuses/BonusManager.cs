using Artaxias.Core.Enums;
using Artaxias.Data;
using Artaxias.Data.Models.Organization;
using Artaxias.Web.Common.DataTransferObjects.Organization;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artaxias.BusinessLogic.Organization.Bonuses
{
    public class BonusManager : IBonusManager
    {
        private readonly IRepository<Bonus> _repository;
        private readonly IRepository<Employee> _employeeRepository;
        private readonly IDomainStateVerifier<Bonus> _domainStateVerifier;

        public BonusManager(IRepository<Bonus> repository,
                            IDomainStateVerifier<Bonus> domainStateVerifier,
                            IRepository<Employee> employeeRepository)
        {
            _repository = repository;
            _domainStateVerifier = domainStateVerifier;
            _employeeRepository = employeeRepository;
        }

        public async Task<BonusResponse> CreateAsync(BonusRequest request)
        {
            //if (_employeeRepository.Get(e => e.Id == request.RequesterId
            //                                 || e.Id == request.ApproverId).Any(e => e.HeadOfDepartment == null))
            //    throw new ArgumentException("Request issued with an unauthorized personal.");

            Bonus bonus = request.Map();
            _repository.Insert(bonus);
            await _repository.SaveChangesAsync();

            return bonus.Map();
        }

        public async Task<BonusResponse> GetAsync(int key)
        {
            IQueryable<Bonus> result = _repository.Get(b => b.Id == key);
            _domainStateVerifier.Verify(result);
            return await result.Map().FirstOrDefaultAsync();
        }

        public async Task<List<BonusResponse>> GetBonusesListForEmployeeAsync(int employeeId, int pageSize = 10, int currentPage = 0)
        {
            IQueryable<Bonus> result = _repository.Get(b => b.ReceiverId == employeeId)
                                    .Where(b => b.DomainStateId != (int)EDomainState.Abandoned)
                                    .OrderByDescending(s => s.CreatedDatetimeUTC)
                                    .Skip(currentPage * pageSize)
                                    .Take(pageSize);
            _domainStateVerifier.Verify(result);
            return await result.Map().ToListAsync();
        }

        public async Task DeleteAsync(int key)
        {
            Bonus bonus = await _repository.Get(b => b.Id == key).FirstOrDefaultAsync();
            Validate(bonus);
            _repository.Delete(bonus);
            await _repository.SaveChangesAsync();
        }

        public async Task<BonusResponse> UpdateAsync(int key, BonusRequest request)
        {
            Bonus bonus = await _repository.Get(b => b.Id == key).FirstOrDefaultAsync();
            Validate(bonus);

            bonus.Amount = request.Amount;
            bonus.PaymentDate = request.PaymentDate;
            bonus.Comment = request.Comment;
            bonus.ApproverId = request.ApproverId;
            bonus.UpdatedDatetimeUTC = DateTime.UtcNow;
            bonus.DomainStateId = request.DomainStateId;

            _repository.Update(bonus);
            await _repository.SaveChangesAsync();

            return bonus.Map();
        }

        private void Validate(Bonus bonus)
        {
            if (bonus == null)
            {
                throw new ArgumentException("Bonus not Found");
            }
        }

        public Task<List<BonusResponse>> GetListAsync(int pageSize = 10, int currentPage = 0)
        {
            throw new NotImplementedException();
        }
    }
}
