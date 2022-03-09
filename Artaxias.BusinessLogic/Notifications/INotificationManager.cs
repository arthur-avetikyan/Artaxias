using Artaxias.Web.Common.DataTransferObjects.Notification;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Artaxias.BusinessLogic.Notifications
{
    public interface INotificationManager
    {
        Task<List<NotificationResponse>> GetUserNotifications(string userName);
    }
}
