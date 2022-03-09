using Artaxias.Web.Client.Services.Interfaces;
using Artaxias.Web.Common.DataTransferObjects.Notification;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Artaxias.Web.Client.Services
{
    public class NotificationApiAccessor : INotificationApiAccessor
    {
        private readonly HttpClient _httpClient;

        public NotificationApiAccessor(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<NotificationResponse>> GetUserNotifications(string userName)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/notifications/{userName}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<NotificationResponse>>();
            }
            else
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }
    }
}
