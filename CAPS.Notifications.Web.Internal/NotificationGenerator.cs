using CAPS.Notifications.Core;
using CAPS.Notifications.Internal;
using CAPS.Notifications.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPS.Notifications.Web.Internal
{
    public class NotificationGenerator
    {
        INotificationRepository repository;
        bool useSignalR;

        public INotificationRepository Repository { get { return repository; } }

        public NotificationGenerator() 
        {
            var config = CAPS.Notifications.Web.Internal.Configuration.NotificationsConfigurationSection.Instance;
            var typeName = config.Repository.RepositoryTypeName;
            var type = Type.GetType(typeName);
            System.Reflection.ConstructorInfo ctor = type.GetConstructor(new Type[] { });
            repository = (INotificationRepository)ctor.Invoke(new object[] { });
            if (config.SignalR != null && config.SignalR.UseSignalR)
                useSignalR = true;
            else throw new Exception("Not using signalR");
        }

        public IEnumerable<Notification> GenerateNotifications(IEnumerable<NewNotification> newNotifications)
        {
            var notifications = repository.AddNotification(newNotifications);
            if (useSignalR)
                foreach (var notification in notifications)
                    SendSignalRNotifcation(notification);
            return notifications;
        }

        private void SendSignalRNotifcation(Notification notification)
        {
            var context = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext("NotificationMessageHub");
            context.Clients.Group("User-" + notification.Username).addNotification(notification);
        }
    }
}
