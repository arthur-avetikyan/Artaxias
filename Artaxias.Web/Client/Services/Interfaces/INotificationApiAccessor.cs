using Artaxias.Web.Common.DataTransferObjects.Notification;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Artaxias.Web.Client.Services.Interfaces
{
    public interface INotificationApiAccessor
    {
        Task<List<NotificationResponse>> GetUserNotifications(string userName);
    }
}
