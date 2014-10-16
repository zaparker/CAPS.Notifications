using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;

namespace CAPS.Notifications.Core
{
    /// <summary>
    /// Helper class for communitcating with the notification service
    /// </summary>
    public class NotificationServiceClient : IDisposable
    {
        private HttpClient client;

        /// <summary>
        /// Creates a new instance of the NotificationServiceClient using the specified url
        /// </summary>
        /// <param name="serviceUrl"></param>
        public NotificationServiceClient(string serviceUrl)
        {
            SetClient(new Uri(serviceUrl));
        }

        /// <summary>
        /// Creates a new instance of the NotificationServiceClient using the specified url
        /// </summary>
        /// <param name="serviceUrl"></param>
        public NotificationServiceClient(Uri serviceUrl)
        {
            SetClient(serviceUrl);
        }
        public HttpClient Client { get { return client; } }

        private void SetClient(Uri serviceUrl)
        {
            client = new HttpClient();
            client.BaseAddress = serviceUrl;
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.Timeout = new TimeSpan(0, 0, 15);
        }

        /// <summary>
        /// Sets the authorization header for the service
        /// </summary>
        /// <param name="scheme"></param>
        /// <param name="parameter"></param>
        public void AddAuthorizationHeader(string scheme, string parameter)
        {
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(scheme, parameter);
        }

        /// <summary>
        /// Gets a pageable list of notifications from the user
        /// </summary>
        /// <param name="username"></param>
        /// <param name="applications"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <param name="read"></param>
        /// <returns></returns>
        public async Task<PageableNotificationResult> GetForUserAsync(string username, string applications, int offset = 0, int limit = 10, bool? read = false)
        {
            if (username == null) // we must always at least filter by user
                throw new ArgumentNullException("username");
            var queryString = "?username=" + username;
            if (!string.IsNullOrWhiteSpace(applications)) queryString += "&applications=" + applications;
            queryString += "&offset=" + offset;
            queryString += "&limit=" + limit;
            if (read != null) queryString += "&read=" + read.Value;
            var response = await client.GetAsync(queryString);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<PageableNotificationResult>();
        }

        /// <summary>
        /// Gets a single notification
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Notification> GetByIdAsync(string id)
        {
            if (id == null)
                throw new ArgumentNullException("id");
            var response = await client.GetAsync(id);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<Notification>();
        }

        /// <summary>
        /// Updates a single notification
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> UpdateAsync(string id, UpdateNotification model)
        {
            if (id == null)
                throw new ArgumentNullException("id");
            if (model == null)
                throw new ArgumentNullException("model");
            var response = await client.PutAsJsonAsync<UpdateNotification>(id, model);
            response.EnsureSuccessStatusCode();
            return response;
        }

        /// <summary>
        /// Disposes the underlying HttpClient inside the NotificationServiceClient
        /// </summary>
        public void Dispose()
        {
            client.Dispose();
        }
    }
}
