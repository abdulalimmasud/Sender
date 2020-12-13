using System;
using System.Collections.Generic;

#nullable disable

namespace AstManagement.AssetDB
{
    public partial class App
    {
        public App()
        {
            AttachmentLogs = new HashSet<AttachmentLog>();
            Attachments = new HashSet<Attachment>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<AttachmentLog> AttachmentLogs { get; set; }
        public virtual ICollection<Attachment> Attachments { get; set; }
    }
}
