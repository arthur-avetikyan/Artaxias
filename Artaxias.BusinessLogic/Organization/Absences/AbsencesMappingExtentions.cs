using Artaxias.Data.Models.Organization;
using Artaxias.Web.Common.DataTransferObjects.Organization;

namespace Artaxias.BusinessLogic.Organization.Absences
{
    internal static class AbsencesMappingExtentions
    {
        public static AbsenceResponse Map(this Absence entity)
        {
            return new AbsenceResponse
            {
                Id = entity.Id,
                Reason = entity.Reason,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                TypeId = entity.TypeId,
                TypeDescription = entity.Type.Description,
                StateId = entity.DomainStateId,
                Receiver = new EmployeeInfo
                {
                    Id = entity.ReceiverId,
                    FirstName = entity.Receiver.User.FirstName,
                    LastName = entity.Receiver.User.LastName
                },
                Approver = new EmployeeInfo
                {
                    Id = entity.ApproverId,
                    FirstName = entity.Approver.User.FirstName,
                    LastName = entity.Approver.User.LastName
                }
            };
        }

        public static Absence Map(this AbsenceRequest request)
        {
            return new Absence
            {
                Reason = request.Reason,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                ReceiverId = request.ReceiverId,
                ApproverId = request.ApproverId,
                TypeId = request.TypeId,
            };
        }

        public static AbsenceTypeResponse Map(this AbsenceType entity)
        {
            return new AbsenceTypeResponse
            {
                Id = entity.Id,
                Description = entity.Description
            };
        }
    }
}
