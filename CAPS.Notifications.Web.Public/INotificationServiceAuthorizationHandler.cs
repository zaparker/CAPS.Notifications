using CAPS.Notifications.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPS.Notifications.Web.Public
{
    public interface INotificationServiceAuthorizationHandler
    {
        Task SetAuthorizationHeaderValueAsync(NotificationServiceClient client);
    }
}
