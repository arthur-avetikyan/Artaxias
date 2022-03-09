using Artaxias.Core.Configurations;
using Artaxias.Core.Enums;
using Artaxias.Data;
using Artaxias.Data.Models.Feadback;
using Artaxias.Data.Models.Organization;
using Artaxias.Web.Common.DataTransferObjects.Notification;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artaxias.BusinessLogic.Notifications
{
    public class NotificationManager : INotificationManager
    {
        private readonly IRepository<ReviewerReviewee> _reviewerRevieweeRepository;
        private readonly IRepository<Absence> _absenceRepository;
        private readonly IRepository<Bonus> _bonusRepository;
        private readonly ApplicationConfiguration _applicationConfiguration;

        public NotificationManager(IRepository<ReviewerReviewee> reviewerRevieweeRepository,
                                   IOptions<ApplicationConfiguration> applicationConfiguration,
                                   IRepository<Absence> absenceRepository,
                                   IRepository<Bonus> bonusRepository)
        {
            _applicationConfiguration = applicationConfiguration.Value;
            _reviewerRevieweeRepository = reviewerRevieweeRepository;
            _absenceRepository = absenceRepository;
            _bonusRepository = bonusRepository;
        }

        // Temporarily developed via plain architecture, ideas on INotificationsProvider interface for each manager
        public async Task<List<NotificationResponse>> GetUserNotifications(string userName)
        {
            List<NotificationResponse> notificationItems = await GetReviewNotificationsAsync(userName);

            notificationItems.AddRange(await GetPendingAbsenceNotificationsAsync(userName));
            notificationItems.AddRange(await GetRejectedAbsenceNotificationsAsync(userName));

            notificationItems.AddRange(await GetRequestedBonusNotificationsAsync(userName));
            notificationItems.AddRange(await GetRejectedOrApprovedBonusNotificationsAsync(userName));

            return notificationItems;
        }

        private async Task<List<NotificationResponse>> GetReviewNotificationsAsync(string userName)
        {
            return await _reviewerRevieweeRepository.Get(rr => rr.Reviewer.User.UserName == userName)
                .Where(rr => rr.DomainStateId == (int)EDomainState.Pending)
                .OrderByDescending(rr => rr.Review.CreatedDateTimeUTC)
                .Map(_applicationConfiguration.Url)
                .ToListAsync();
        }

        private async Task<List<NotificationResponse>> GetPendingAbsenceNotificationsAsync(string userName)
        {
            return await _absenceRepository.Get(a => a.Approver.User.UserName == userName)
                .Where(a => a.DomainStateId == (int)EDomainState.Pending)
                .OrderByDescending(rr => rr.CreatedDatetimeUTC)
                .Map(_applicationConfiguration.Url)
                .ToListAsync();
        }

        private async Task<List<NotificationResponse>> GetRejectedAbsenceNotificationsAsync(string userName)
        {
            return await _absenceRepository.Get(a => a.Receiver.User.UserName == userName)
                .Where(a => a.DomainStateId == (int)EDomainState.Rejected)
                .OrderByDescending(rr => rr.CreatedDatetimeUTC)
                .MapToRejection(_applicationConfiguration.Url)
                .ToListAsync();
        }

        private async Task<List<NotificationResponse>> GetRequestedBonusNotificationsAsync(string userName)
        {
            return await _bonusRepository.Get(b => b.Approver.User.UserName == userName)
                .Where(b => b.DomainStateId == (int)EDomainState.Requested)
                .OrderByDescending(b => b.CreatedDatetimeUTC)
                .MapBonusRequestNotification(_applicationConfiguration.Url)
                .ToListAsync();
        }

        private async Task<List<NotificationResponse>> GetRejectedOrApprovedBonusNotificationsAsync(string userName)
        {
            return await _bonusRepository.Get(b => b.Receiver.User.UserName == userName || b.Requester.User.UserName == userName)
                .Where(b => b.DomainStateId == (int)EDomainState.Rejected || b.DomainStateId == (int)EDomainState.Approved)
                .OrderByDescending(b => b.CreatedDatetimeUTC)
                .MapBonusRequestResponseNotification(_applicationConfiguration.Url)
                .ToListAsync();
        }
    }
}
