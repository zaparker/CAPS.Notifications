using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CAPS.Notifications.Core
{
    /// <summary>
    /// A pageable result object containing a page of entries as well as the total entries
    /// </summary>
    [DataContract(Namespace = Constants.DataContractNamespace)]
    public class PageableNotificationResult
    {
        /// <summary>
        /// This page's offset into the result
        /// </summary>
        [JsonProperty(PropertyName = "offset")]
        [DataMember]
        public int Offset { get; set; }

        /// <summary>
        /// The max number of entries in this page
        /// </summary>
        [JsonProperty(PropertyName = "limit")]
        [DataMember]
        public int Limit { get; set; }

        /// <summary>
        /// The total number of notifications for this result
        /// </summary>
        [JsonProperty(PropertyName = "total")]
        [DataMember]
        public int Total { get; set; }

        /// <summary>
        /// The current page of results
        /// </summary>
        [JsonProperty(PropertyName = "results")]
        [DataMember]
        public IEnumerable<Notification> Results { get; set; }
    }
}
