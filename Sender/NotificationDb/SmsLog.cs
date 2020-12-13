using System;
using System.Collections.Generic;

namespace NotificationDb
{
    public partial class SmsLog
    {
        public long Id { get; set; }
        public string MobileNumber { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public string ExceptionMessage { get; set; }
        public int AppId { get; set; }
        public int? IsResolved { get; set; }
        public DateTime LogTime { get; set; }
        public string ResponseJson { get; set; }
    }
}
