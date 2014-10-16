using CAPS.Notifications.Core;
using CAPS.Notifications.Internal;
using CAPS.Notifications.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace CAPS.Notifications.Web.Internal
{
    /// <summary>
    /// Represents an public-facing notification service that can get notifications but not add new ones. Passes messages to the internal notification service. 
    /// </summary>
    [RoutePrefix("api/notifications")]
    public class NotificationController : ApiController
    {
        NotificationGenerator generator;

        public NotificationController() 
        {
            generator = new NotificationGenerator();
        }

        [Route("")]
        [HttpGet]
        public PageableNotificationResult GetForUser([FromUri]string username = null, [FromUri]string applications = null, [FromUri] int? offset = 0, [FromUri]int? limit = 10, [FromUri] bool? read = null)
        {
            if (username == null) // we must always at least filter by user
                throw new HttpResponseException(HttpStatusCode.BadRequest);
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

        [Route("{id}", Name = "GetById")]
        [HttpGet]
        public Notification GetById([FromUri]string id)
        {
            var notification = generator.Repository.GetSingleNotification(id);
            if (notification == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            return notification;
        }

        [Route("{id}")]
        [HttpPut]
        public HttpResponseMessage Put([FromUri] string id, [FromBody] UpdateNotification model)
        {
            if (model == null)
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            var notification = generator.Repository.GetSingleNotification(id);
            notification.Read = model.Read;
            generator.Repository.UpdateNotification(notification);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [Route("")]
        [HttpPost]
        public HttpResponseMessage Post([FromBody] NewNotification model)
        {
            if (model == null)
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            var notification = generator.GenerateNotifications(new NewNotification[] { model }).First();
            var response = Request.CreateResponse<Notification>(HttpStatusCode.Created, notification);
            var route = Url.Route("GetById", new { id = notification.Id });
            response.Headers.Location = new Uri(route, UriKind.Relative);
            return response;
        }

        [Route("")]
        [HttpPut]
        public HttpResponseMessage Put([FromBody] IEnumerable<NewNotification> model)
        {
            if (model == null)
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            var notifications = generator.GenerateNotifications(model);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
