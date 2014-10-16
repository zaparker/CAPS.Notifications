using CAPS.Notifications.Core;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPS.Notifications.Web.Internal
{
    public class NotificationMessageHub : Hub
    {
         NotificationGenerator generator;


        public NotificationMessageHub()
        {
            generator = new NotificationGenerator();
        }

        public void JoinGroup(string groupName) // to always send just to this user, use the "User-{username}" as the group name
        {
            Groups.Add(Context.ConnectionId, groupName);
        }

        public PageableNotificationResult GetForUser(string username, string applications = null, int? offset = 0, int? limit = 10, bool? read = false)
        {
            var results = generator.Repository.GetNotificationsQueryable().Where(n => n.Username == username);
            if (!string.IsNullOrWhiteSpace(applications))
                results = results.Where(n => applications.Contains(n.Application));
            var total = results.Count();
            if (read != null)
                results = results.Where(n => n.Read == read.Value);
            results = results.OrderByDescending(n => n.DateAdded).Skip(offset.Value).Take(limit.Value);
            return new PageableNotificationResult
            {
                Offset = offset.Value,
                Limit = limit.Value,
                Total = total,
                Results = results.ToArray()
            };
        }

        public void MarkAsRead(string id)
        {
            var notification = generator.Repository.GetSingleNotification(id);
            notification.Read = true;
            generator.Repository.UpdateNotification(notification);
        }
    }
}
