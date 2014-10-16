using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CAPS.Notifications.Core
{
    /// <summary>
    /// Updates a notification
    /// </summary>
    [DataContract(Namespace = Constants.DataContractNamespace)]
    public class UpdateNotification
    {
        /// <summary>
        /// Indicates whether or not the user has previously read this notification
        /// </summary>
        [JsonProperty(PropertyName = "read")]
        [DataMember]
        public bool Read { get; set; }
    }
}
