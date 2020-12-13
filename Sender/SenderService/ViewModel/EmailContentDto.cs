using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenderService.ViewModel
{
    public class EmailContentDto
    {
        public EmailContentDto(string recipientEmail, string subject, string messageHTML)
        {
            To = recipientEmail;
            Subject = subject;
            MessageHTML = messageHTML;
        }
        public string To { get; private set; }
        public string Subject { get; private set; }
        public string MessageHTML { get; private set; }
        public string From
        {
            get
            {
                return "notification@mail.eyeelectronics.net";
            }
        }
        //public Encoding BodyEncoding
        //{
        //    get
        //    {
        //        return Encoding.GetEncoding("utf-8");
        //    }
        //}
        //public string Priority
        //{
        //    get
        //    {
        //        return "High";
        //    }
        //}
        public bool IsBodyHtml
        {
            get
            {
                return true;
            }
        }
    }
}
