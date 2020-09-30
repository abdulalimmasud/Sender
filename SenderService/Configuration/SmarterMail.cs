using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SenderService.Configuration
{
    public class SmarterMail
    {
        public string AuthApiUrl { get; set; }
        public string SendEmailApiUrl { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
