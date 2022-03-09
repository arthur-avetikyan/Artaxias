using Artaxias.BusinessLogic.Notifications;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Threading.Tasks;

namespace Artaxias.Web.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationManager _notificationManager;

        public NotificationsController(INotificationManager notificationManager)
        {
            _notificationManager = notificationManager;
        }

        [HttpGet("{userName}")]
        public async Task<IActionResult> Get(string userName)
        {
            try
            {
                if (userName != User.Identity.Name)
                {
                    return BadRequest("Usernames does not match.");
                }

                System.Collections.Generic.List<Common.DataTransferObjects.Notification.NotificationResponse> result = await _notificationManager.GetUserNotifications(userName);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
