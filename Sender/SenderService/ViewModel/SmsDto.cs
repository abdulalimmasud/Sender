using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SenderService.ViewModel
{
    public class SmsDto:Common
    {
        public string MobileNumber { get; set; }
        public int AppId { get; set; }
    }
}
