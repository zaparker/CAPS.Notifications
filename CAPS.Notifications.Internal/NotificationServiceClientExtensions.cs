using CAPS.Notifications.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CAPS.Notifications.Internal
{
    public static class NotificationServiceClientExtensions
    {
        public static async Task<HttpResponseMessage> AddSingle(this NotificationServiceClient client, NewNotification model)
        {
            var response = await client.Client.PostAsJsonAsync<NewNotification>("", model);
            response.EnsureSuccessStatusCode();
            return response;
        }

        public static async Task<HttpResponseMessage> AddMany(this NotificationServiceClient client, IEnumerable<NewNotification> model)
        {
            var response = await client.Client.PutAsJsonAsync<IEnumerable<NewNotification>>("", model);
            response.EnsureSuccessStatusCode();
            return response;
        }
    }
}
