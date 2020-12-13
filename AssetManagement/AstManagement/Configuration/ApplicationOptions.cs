using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AstManagement.Configuration
{
    public class ApplicationOptions
    {
        public List<string> Whitelist { get; set; }
        public string ApiKey { get; set; }
    }
}
