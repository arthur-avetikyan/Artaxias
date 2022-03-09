using Artaxias.Data.Models.Feadback;
using Artaxias.Data.Models.Organization;
using Artaxias.Web.Common.DataTransferObjects.Notification;

using System.Linq;

namespace Artaxias.BusinessLogic.Notifications
{
    internal static class NotificationsMappingExtentions
    {
        public static IQueryable<NotificationResponse> Map(this IQueryable<ReviewerReviewee> entity, string baseUrl)
        {
            return entity.Select(entity => new NotificationResponse
            {
                Category = "Feedback",
                Title = "Feedback request",
                Message = $"Deadline: {entity.Review.Deadline:MMMM dd, yyyy}",
                Url = $"{baseUrl}/feedbacks/{entity.Reviewer.User.UserName}/{entity.Id}"
            });
        }

        public static IQueryable<NotificationResponse> Map(this IQueryable<Absence> entity, string baseUrl)
        {
            return entity.Select(entity => new NotificationResponse
            {
                Category = "Absence",
                Title = "Absence request",
                Message = $"By: {entity.Receiver.User.FirstName} {entity.Receiver.User.LastName}",
                Url = $"{baseUrl}/absences/{entity.Id}"
            });
        }

        public static IQueryable<NotificationResponse> MapToRejection(this IQueryable<Absence> entity, string baseUrl)
        {
            return entity.Select(entity => new NotificationResponse
            {
                Category = "Absence",
                Title = "Absence rejection",
                Message = $"By: {entity.Approver.User.FirstName} {entity.Approver.User.LastName}",
                Url = $"{baseUrl}/absences/{entity.Id}"
            });
        }

        public static IQueryable<NotificationResponse> MapBonusRequestNotification(this IQueryable<Bonus> entity, string baseUrl)
        {
            return entity.Select(entity => new NotificationResponse
            {
                Category = "Bonus",
                Title = $"Bonus requested",
                Message = $"Bonus for {entity.Receiver.User.FirstName} {entity.Receiver.User.LastName} "
                          + $"requested by {entity.Requester.User.FirstName} {entity.Requester.User.LastName}:",
                Url = $"{baseUrl}/bonuses/{entity.Id}"
            });
        }

        public static IQueryable<NotificationResponse> MapBonusRequestResponseNotification(this IQueryable<Bonus> entity, string baseUrl)
        {
            return entity.Select(entity => new NotificationResponse
            {
                Category = "Bonus",
                Title = $"Bonus {entity.State.Description}",
                Message = $"By {entity.Approver.User.FirstName} {entity.Approver.User.LastName} ",
                Url = $"{baseUrl}/bonuses/{entity.Id}"
            });
        }
    }
}
