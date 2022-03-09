using Artaxias.Core.Enums;
using Artaxias.Data.Models.Organization;
using Artaxias.Web.Common.DataTransferObjects.Organization;

using System;
using System.Linq;

namespace Artaxias.BusinessLogic.Organization.Bonuses
{
    public static class BonusMappingExtensions
    {
        public static IQueryable<BonusResponse> Map(this IQueryable<Bonus> entity)
        {
            return entity.Select(b => new BonusResponse
            {
                Id = b.Id,
                Amount = b.Amount,
                Comment = b.Comment,
                PaymentDate = b.PaymentDate,
                DomainStateId = b.DomainStateId,
                Approver = new EmployeeInfo
                {
                    Id = b.ApproverId,
                    FirstName = b.Approver.User.FirstName,
                    LastName = b.Approver.User.LastName
                },
                Receiver = new EmployeeInfo
                {
                    Id = b.ReceiverId,
                    FirstName = b.Receiver.User.FirstName,
                    LastName = b.Receiver.User.LastName
                },
                Requester = new EmployeeInfo
                {
                    Id = b.RequesterId,
                    FirstName = b.Requester.User.FirstName,
                    LastName = b.Requester.User.LastName
                }
            });
        }

        public static BonusResponse Map(this Bonus entity)
        {
            return new BonusResponse
            {
                Id = entity.Id,
                Amount = entity.Amount,
                Comment = entity.Comment,
                PaymentDate = entity.PaymentDate,
                DomainStateId = entity.DomainStateId,
                Approver = new EmployeeInfo
                {
                    Id = entity.ApproverId,
                    FirstName = entity.Approver.User.FirstName,
                    LastName = entity.Approver.User.LastName
                },
                Receiver = new EmployeeInfo
                {
                    Id = entity.ReceiverId,
                    FirstName = entity.Receiver.User.FirstName,
                    LastName = entity.Receiver.User.LastName
                },
                Requester = new EmployeeInfo
                {
                    Id = entity.RequesterId,
                    FirstName = entity.Requester.User.FirstName,
                    LastName = entity.Requester.User.LastName
                }
            };
        }

        public static Bonus Map(this BonusRequest request)
        {
            Bonus bonus = new Bonus
            {
                Amount = request.Amount,
                PaymentDate = request.PaymentDate,
                Comment = request.Comment,
                ApproverId = request.ApproverId,
                ReceiverId = request.ReceiverId,
                RequesterId = request.RequesterId
            };
            if (request.Id > 0)
            {
                bonus.Id = request.Id;
                bonus.UpdatedDatetimeUTC = DateTime.UtcNow;
                bonus.DomainStateId = request.DomainStateId;
            }
            else
            {
                bonus.CreatedDatetimeUTC = DateTime.UtcNow;
                bonus.DomainStateId = (int)EDomainState.Requested;
            }

            return bonus;
        }
    }
}
