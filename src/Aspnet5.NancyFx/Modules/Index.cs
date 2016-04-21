using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4.Modules
{
    public class Index : Nancy.NancyModule
    {
        public Index()
        {
            Get["/"] = (_,t) => Task.FromResult<dynamic>("sdsds"); 
        }
    }
}
