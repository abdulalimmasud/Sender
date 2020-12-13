using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SenderService.ViewModel
{
    public class MailDto:Common
    {
        public string RecipientEmail { get; set; }
        public string RecipientName { get; set; }
        public string Subject { get; set; }
        
    }
    public class EmailDto: MailDto
    {
        public int? CustomerId { get; set; }
        public int AppId { get; set; }
    }
    public class BulkEmailDto
    {
        public int? CustomerId { get; set; }
        public int AppId { get; set; }
        public List<MailDto> Mails { get; set; }
    }
}
