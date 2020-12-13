using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SenderService.ViewModel
{
    public class ServiceClass
    {
        public string MessageId { get; set; }
        public int Status { get; set; }
        public string StatusText { get; set; }
        public int ErrorCode { get; set; }
        public string ErrorText { get; set; }
        public int SMSCount { get; set; }
        public double CurrentCredit { get; set; }
    }
    public class ArrayOfServiceClass
    {
        public ServiceClass ServiceClass { get; set; }
    }
}
