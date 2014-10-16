using CAPS.Notifications.Core;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPS.Notifications.Web.Public
{
    public class NotificationMessageHub : Hub
    {
        private string internalServiceUrl;
        private string authHeaderScheme, authHeaderValue;

        public NotificationMessageHub()
        {
            var config = CAPS.Notifications.Web.Public.Configuration.NotificationsConfigurationSection.Instance;
            internalServiceUrl = config.Service.Url;
        }

        public void JoinGroup(string groupName) // to always send just to this user, use the "User-{username}" as the group name
        {
            Groups.Add(Context.ConnectionId, groupName);
        }

        public async Task<PageableNotificationResult> GetForUser(string username, string applications = null, int? offset = 0, int? limit = 10, bool? read = false)
        {
            using (var client = new NotificationServiceClient(internalServiceUrl))
            {
                var config = CAPS.Notifications.Web.Public.Configuration.NotificationsConfigurationSection.Instance;
                if (config.Service.UseAuthorizationHeaders)
                {
                    var typeName = config.Service.AuthorizationHeaderHandlerName;
                    if (string.IsNullOrWhiteSpace(typeName))
                        throw new Exception("Missing Notifications AuthorizationHeaderHandlerName");
                    var type = Type.GetType(typeName);
                    System.Reflection.ConstructorInfo ctor = type.GetConstructor(new Type[] { });
                    var handler = (INotificationServiceAuthorizationHandler)ctor.Invoke(new object[] { });
                    await handler.SetAuthorizationHeaderValueAsync(client);
                }
                return await client.GetForUserAsync(username, applications, offset.Value, limit.Value, read);
            }
        }

        public async Task MarkAsRead(string id)
        {
            using (var client = new NotificationServiceClient(internalServiceUrl))
            {
                var config = CAPS.Notifications.Web.Public.Configuration.NotificationsConfigurationSection.Instance;
                if (config.Service.UseAuthorizationHeaders)
                {
                    var typeName = config.Service.AuthorizationHeaderHandlerName;
                    if (string.IsNullOrWhiteSpace(typeName))
                        throw new Exception("Missing Notifications AuthorizationHeaderHandlerName");
                    var type = Type.GetType(typeName);
                    System.Reflection.ConstructorInfo ctor = type.GetConstructor(new Type[] { });
                    var handler = (INotificationServiceAuthorizationHandler)ctor.Invoke(new object[] { });
                    await handler.SetAuthorizationHeaderValueAsync(client);
                }
                await client.UpdateAsync(id, new UpdateNotification { Read = true });
            }
        }
    }
}
