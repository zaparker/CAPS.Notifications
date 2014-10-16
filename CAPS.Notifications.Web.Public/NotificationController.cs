using CAPS.Notifications.Core;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace CAPS.Notifications.Web.Public
{
    /// <summary>
    /// Represents an internal notification service that can get notifications and add new ones
    /// </summary>
    [RoutePrefix("api/notifications")]
    public class NotificationController : ApiController
    {
        private string internalServiceUrl;
        private string authHeaderScheme, authHeaderValue;

        public NotificationController()
        {
            var config = CAPS.Notifications.Web.Public.Configuration.NotificationsConfigurationSection.Instance;
            internalServiceUrl = config.Service.Url;
        }
        [Route("")]
        [HttpGet]
        public async Task<PageableNotificationResult> GetForUser([FromUri]string username = null, [FromUri]string applications = null, [FromUri] int? offset = 0, [FromUri]int? limit = 10, [FromUri] bool? read = null)
        {
            if (username == null) // we must always at least filter by user
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            using (var client = new NotificationServiceClient(internalServiceUrl))
            {
                var config = CAPS.Notifications.Web.Public.Configuration.NotificationsConfigurationSection.Instance;
                internalServiceUrl = config.Service.Url;
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
        [Route("{id}")]
        [HttpGet]
        public async Task<Notification> GetById([FromUri]string id)
        {
            using (var client = new NotificationServiceClient(internalServiceUrl))
            {
                var config = CAPS.Notifications.Web.Public.Configuration.NotificationsConfigurationSection.Instance;
                internalServiceUrl = config.Service.Url;
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
                return await client.GetByIdAsync(id);
            }
        }
        [Route("{id}")]
        [HttpPut]
        public async Task<HttpResponseMessage> Put([FromUri] string id, [FromBody] UpdateNotification model)
        {
            if (model == null)
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            using (var client = new NotificationServiceClient(internalServiceUrl))
            {
                var config = CAPS.Notifications.Web.Public.Configuration.NotificationsConfigurationSection.Instance;
                internalServiceUrl = config.Service.Url;
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
                await client.UpdateAsync(id, model);
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
