using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace CAPS.Notifications.Core
{
    /// <summary>
    /// A single user notification
    /// </summary>
    [DataContract(Namespace = Constants.DataContractNamespace)]
    public class Notification
    {
        /// <summary>
        /// Notification ids should be unique across all users
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        [DataMember]
        public string Id { get; set; }

        /// <summary>
        /// Required. The user to which this notification is associated
        /// </summary>
        [JsonProperty(PropertyName = "username")]
        [DataMember]
        public string Username { get; set; }

        /// <summary>
        /// Required. A short description of the notification
        /// </summary>
        [JsonProperty(PropertyName = "title")]
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// Required. The verbose description of the notification
        /// </summary>
        [JsonProperty(PropertyName = "text")]
        [DataMember]
        public string Text { get; set; }


        /// <summary>
        /// Required. The application which generated the notification
        /// </summary>
        [JsonProperty(PropertyName = "application")]
        [DataMember]
        public string Application { get; set; }

        /// <summary>
        /// Optional. An image to display alongside the notification text and title
        /// </summary>
        [JsonProperty(PropertyName = "imageAbsoluteUrl")]
        [DataMember]
        public string ImageAbsoluteUrl { get; set; }

        /// <summary>
        /// Optional. An in-app route that should to determine app behavior when the user clicks the notification link
        /// </summary>
        [JsonProperty(PropertyName = "linkRoute")]
        [DataMember]
        public string LinkRoute { get; set; }

        /// <summary>
        /// Optional. A web url to use when the user clicks the notification link if the app does not handle routes
        /// </summary>
        [JsonProperty(PropertyName = "linkFallbackAbsoluteUrl")]
        [DataMember]
        public string LinkFallbackAbsoluteUrl { get; set; }

        /// <summary>
        /// Indicates whether or not the user has previously read this notification
        /// </summary>
        [JsonProperty(PropertyName = "read")]
        [DataMember]
        public bool Read { get; set; }

        /// <summary>
        /// The date and time at which the notification was created
        /// </summary>
        [JsonProperty(PropertyName = "dateAdded")]
        [DataMember]
        public DateTime DateAdded { get; set; }
    }
}
