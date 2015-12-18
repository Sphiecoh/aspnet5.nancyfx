using AspNet.Identity.MongoDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4.Models
{
    public class User : IdentityUser
    {
        public User()
        {
            EmailNotificationsEnabled = true;
        }
        /// <summary>
        /// A list of tenenants that the user belongs to
        /// </summary>
        public ICollection<string> TenantIds { get; set; }
        /// <summary>
        /// Gets or sets the users Full Name.
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// Subscribe to notifications
        /// </summary>
        public bool EmailNotificationsEnabled { get; set; }
              
    }
}
