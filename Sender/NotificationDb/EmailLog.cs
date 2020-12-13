using System;
using System.Collections.Generic;

namespace NotificationDb
{
    public partial class EmailLog
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string RecipientName { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public int? CustomerId { get; set; }
        public int StatusCode { get; set; }
        public string ExceptionMessage { get; set; }
        public int AppId { get; set; }
        public int? IsResolved { get; set; }
        public DateTime LogTime { get; set; }
    }
}
