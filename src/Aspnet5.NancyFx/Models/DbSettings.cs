using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aspnet5.NancyFx.Models
{
    public class DbSettings
    {
        public string ConnectionString { get; set; }
        public string Database { get; set; }
        public string UserTable { get; set; }
        public string RoleTable { get; set; }
    }
}
