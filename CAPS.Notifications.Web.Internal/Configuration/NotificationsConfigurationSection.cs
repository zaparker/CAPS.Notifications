using System;
using System.Collections.Generic;
using System.Configuration;

namespace CAPS.Notifications.Web.Internal.Configuration
{
    public class NotificationsConfigurationSection : ConfigurationSection
    {
        public static NotificationsConfigurationSection Instance
        {
            get 
            {
                var section = (NotificationsConfigurationSection)System.Configuration.ConfigurationManager.GetSection("notifications");
                if(section == null)
                    throw new Exception("Missing configuration section \"notifications\"");
                return section;
            }
        }

        [ConfigurationProperty("repository", IsRequired = true)]
        public NotifcationsRepositoryConfigurationElement Repository
        {
            get
            {
                return (NotifcationsRepositoryConfigurationElement)base["repository"];
            }
        }

        [ConfigurationProperty("signalR", IsRequired = false)]
        public NotifcationsSignalRConfigurationElement SignalR
        {
            get
            {
                return (NotifcationsSignalRConfigurationElement)base["signalR"];
            }
        }
    }

    public class NotifcationsRepositoryConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("repositoryTypeName", IsRequired = true)]
        public string RepositoryTypeName
        {
            get { return (string)base["repositoryTypeName"]; }
            set { base["repositoryTypeName"] = value; }
        }
    }

    public class NotifcationsSignalRConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("useSignalR", IsRequired = false)]
        public bool UseSignalR
        {
            get { return (bool)base["useSignalR"]; }
            set { base["useSignalR"] = value; }
        }
    }
}
