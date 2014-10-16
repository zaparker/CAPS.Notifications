using CAPS.Notifications.Core;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPS.Notifications.Repository.EF
{
    public class NotificationRepository : INotificationRepository
    {
        private NotificationsContext context;

        public NotificationRepository()
        {
            context = new NotificationsContext();
        }

        public IQueryable<Core.Notification> GetNotificationsQueryable()
        {
            return context.Notifications;
        }

        public Core.Notification GetSingleNotification(string id)
        {
            return context.Notifications.FirstOrDefault(n => n.Id == id);
        }


        private Notification CreateEntity(Internal.NewNotification newNotification)
        {
            return new Notification
            {
                Id = Guid.NewGuid().ToString(),
                Application = newNotification.Application,
                DateAdded = DateTime.Now,
                ImageAbsoluteUrl = newNotification.ImageAbsoluteUrl,
                LinkFallbackAbsoluteUrl = newNotification.LinkFallbackAbsoluteUrl,
                LinkRoute = newNotification.LinkRoute,
                Text = newNotification.Text,
                Title = newNotification.Title,
                Username = newNotification.Username,
                Read = false
            };
        }

        public Core.Notification AddNotification(Internal.NewNotification newNotification)
        {
            var notification = CreateEntity(newNotification);
            context.Notifications.Add(notification);
            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                var errorMessage = string.Empty;
                foreach (var error in ex.EntityValidationErrors)
                {
                    errorMessage += string.Join(", ", error.ValidationErrors.Select(s => s.PropertyName + ":" + s.ErrorMessage));
                    errorMessage += ", ";
                }
                throw new Exception("Validation Failed: " + errorMessage);
            }
            return notification;
        }

        public IEnumerable<Core.Notification> AddNotification(IEnumerable<Internal.NewNotification> newNotifications)
        {
            var notifications = newNotifications.Select(CreateEntity);
            context.Notifications.AddRange(notifications);
            context.SaveChanges();
            return notifications;
        }

        public void UpdateNotification(Core.Notification notification)
        {
            var original = context.Notifications.First(n => n.Id == notification.Id);
            original.Read = notification.Read;
            context.SaveChanges();
        }

        public void DeleteNotification(string id)
        {
            context.Notifications.Remove(context.Notifications.First(n => n.Id == id));
            context.SaveChanges();
        }
    }
}
