using Artaxias.Core.Enums;
using Artaxias.Core.Exceptions;
using Artaxias.Data;
using Artaxias.Data.Models.Organization;
using Artaxias.Web.Common.DataTransferObjects.Organization;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artaxias.BusinessLogic.Organization.Absences
{
    public class AbsenceManager : IAbsencesManager
    {
        private readonly IRepository<Absence> _absenceRepository;
        private readonly IRepository<AbsenceType> _absenceTypeRepository;
        private readonly IRepository<Employee> _employeeRepository;
        private readonly IDomainStateVerifier<Absence> _absenceDomainStateVerifier;

        public AbsenceManager(
            IRepository<Absence> absenceRepository,
            IRepository<AbsenceType> absenceTypeRepository,
            IRepository<Employee> employeeRepository,
            IDomainStateVerifier<Absence> absenceDomainStateVerifier)
        {
            _absenceRepository = absenceRepository;
            _absenceTypeRepository = absenceTypeRepository;
            _employeeRepository = employeeRepository;
            _absenceDomainStateVerifier = absenceDomainStateVerifier;
        }

        public async Task<AbsenceResponse> GetAsync(int id)
        {
            Absence result = await _absenceRepository
                .Get(s => s.Id == id)
                .Include(x => x.Approver.User)
                .Include(x => x.Receiver.User)
                .Include(x => x.Type)
                .FirstOrDefaultAsync();

            _absenceDomainStateVerifier.Verify(result);

            return result.Map();
        }

        public Task<List<AbsenceResponse>> GetListAsync(int pageSize = 10, int currentPage = 0)
        {
            throw new NotImplementedException();
        }

        public async Task<List<AbsenceResponse>> GetListAsync(int employeeId, int pageSize = 10, int currentPage = 0)
        {
            IQueryable<Absence> absences = employeeId == 0 ?
                _absenceRepository.Get() :
                _absenceRepository.Get(s => s.ReceiverId == employeeId);
            List<Absence> results = await absences
                .Include(x => x.Approver.User)
                .Include(x => x.Receiver.User)
                .Include(x => x.Type)
                .Skip(currentPage * pageSize)
                .Take(pageSize)
                .OrderByDescending(s => s.StartDate)
                .ToListAsync();

            _absenceDomainStateVerifier.Verify(results);

            return results.Select(a => a.Map()).ToList();
        }

        public async Task DeleteAsync(int key)
        {
            if (key == 0)
            {
                throw new ArgumentNullException(nameof(key), $"{nameof(key)} is null");
            }
            Absence absence = await _absenceRepository.Get(s => s.Id == key).FirstOrDefaultAsync();
            if (absence == null)
            {
                throw new ArgumentException($"An {nameof(Absence)} doesn't exist for {nameof(key)}: {key}");
            }
            _absenceRepository.Delete(absence);
            await _absenceRepository.SaveChangesAsync();
        }

        public async Task<List<AbsenceTypeResponse>> GetAllTypesAsync(int pageSize = 10, int currentPage = 0)
        {
            return await _absenceTypeRepository
                .Get()
                .Skip(currentPage * pageSize)
                .Take(pageSize)
                .OrderBy(s => s.Id)
                .Select(a => a.Map())
                .ToListAsync();
        }

        public async Task ApproveAsync(int id, int manageId)
        {
            Absence absence = await _absenceRepository
                .Get(x => x.Id == id)
                .FirstOrDefaultAsync();
            if (absence.DomainStateId == (int)EDomainState.Approved)
            {
                throw new DomainException("The absence is already approved");
            }

            absence.DomainStateId = (int)EDomainState.Approved;
            _absenceRepository.Update(absence);
            await _absenceRepository.SaveChangesAsync();
        }

        public async Task RejectAsync(int id, int manageId)
        {
            Absence absence = await _absenceRepository
                .Get(x => x.Id == id)
                .FirstOrDefaultAsync();

            absence.DomainStateId = (int)EDomainState.Rejected;
            _absenceRepository.Update(absence);
            await _absenceRepository.SaveChangesAsync();
        }

        public async Task<AbsenceResponse> CreateAsync(AbsenceRequest request)
        {
            // ReSharper disable once IdentifierTypo
            Employee approver = await _employeeRepository
                .Get(x => x.Id == request.ApproverId)
                //  .Include(x => x.HeadOfDepartment)
                .FirstOrDefaultAsync();
            //if (approver?.HeadOfDepartment == null) // Checks weather the mentioned approver is head of any department
            //{
            //    throw new DomainException("The approver employee is not valid for the absence request");
            //}
            Absence absence = request.Map();
            absence.DomainStateId = (int)EDomainState.Pending;
            _absenceRepository.Insert(absence);
            await _absenceRepository.SaveChangesAsync();

            return await GetAsync(absence.Id);
        }

        public async Task<AbsenceResponse> UpdateAsync(int key, AbsenceRequest request)
        {
            Absence absence = request.Map();
            absence.Id = key;
            absence.DomainStateId = (int)EDomainState.Pending;
            _absenceRepository.Update(absence);
            await _absenceRepository.SaveChangesAsync();

            return absence.Map();
        }
    }
}
