using CAPS.Notifications.Core;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPS.Notifications.Repository.EF
{
    class NotificationsContext : DbContext
    {
        public NotificationsContext()
        { }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Properties().Where(p => p.Name == "Id").Configure(p => p.IsKey()); 
        }
    }
}
