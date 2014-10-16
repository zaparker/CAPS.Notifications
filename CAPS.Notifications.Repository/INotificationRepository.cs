using CAPS.Notifications.Core;
using CAPS.Notifications.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPS.Notifications.Repository
{
    public interface INotificationRepository
    {
        IQueryable<Notification> GetNotificationsQueryable();

        Notification GetSingleNotification(string id);

        Notification AddNotification(NewNotification newNotification);

        IEnumerable<Notification> AddNotification(IEnumerable<NewNotification> newNotifications);

        void UpdateNotification(Notification notification);

        void DeleteNotification(string id);
    }
}
