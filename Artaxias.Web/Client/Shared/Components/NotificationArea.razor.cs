using Artaxias.Web.Client.Services.Interfaces;
using Artaxias.Web.Common.DataTransferObjects.Notification;

using MatBlazor;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Artaxias.Web.Client.Shared.Components
{
    public partial class NotificationArea
    {
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private INotificationApiAccessor NotificationApiAccessor { get; set; }

        [Parameter] public string UserName { get; set; }

        private readonly ForwardRef buttonForwardRef = new ForwardRef();
        private BaseMatMenu Menu;

        private List<NotificationResponse> _notificationItems = new List<NotificationResponse>();
        private int _notificationsCount;
        private bool HasNotification => _notificationsCount > 0;
        private HashSet<string> categories;

        protected override async Task OnInitializedAsync()
        {
            if (!string.IsNullOrWhiteSpace(UserName))
            {
                await GetNotificationsAsync();
                InitializeNotifiactionItems();
                StateHasChanged();
            }
        }

        private void InitializeNotifiactionItems()
        {
            _notificationsCount = _notificationItems.Count;
            categories = new HashSet<string>();
            foreach (NotificationResponse item in _notificationItems)
            {
                categories.Add(item.Category);
            }
        }

        private async Task GetNotificationsAsync()
        {
            try
            {
                _notificationItems = await NotificationApiAccessor.GetUserNotifications(UserName);
            }
            catch (Exception)
            {
                //TO HANDLE
            }
        }

        public void OnClick(MouseEventArgs e)
        {
            Menu.OpenAsync();
        }

        private void OpenNotification(string url)
        {
            NavigationManager.NavigateTo(url, true);
        }
    }
}
