using System;
using System.Collections.Generic;
using System.Configuration;

namespace CAPS.Notifications.Web.Public.Configuration
{
    public class NotificationsConfigurationSection : ConfigurationSection
    {
        public static NotificationsConfigurationSection Instance
        {
            get 
            {
                var section = (NotificationsConfigurationSection)System.Configuration.ConfigurationManager.GetSection("notifications");
                if (section == null)
                    throw new Exception("Missing configuration section \"notifications\"");
                return section;
            }
        }


        [ConfigurationProperty("service")]
        public NotifcationsServiceConfigurationElement Service
        {
            get
            {
                return (NotifcationsServiceConfigurationElement)base["service"];
            }
        }
        
    }

    public class NotifcationsServiceConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("url", IsRequired = true)]
        public string Url
        {
            get { return (string)base["url"]; }
            set { base["url"] = value; }
        }

        [ConfigurationProperty("useAuthorizationHeaders", IsRequired = false, DefaultValue = false)]
        public bool UseAuthorizationHeaders
        {
            get { return (bool)base["useAuthorizationHeaders"]; }
            set { base["useAuthorizationHeaders"] = value; }
        }

        [ConfigurationProperty("authorizationHandlerName", IsRequired = false)]
        public string AuthorizationHeaderHandlerName
        {
            get { return (string)base["authorizationHandlerName"]; }
            set { base["authorizationHandlerName"] = value; }
        }
    }
}
