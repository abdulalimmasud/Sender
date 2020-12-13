using System;
using System.Collections.Generic;

#nullable disable

namespace AstManagement.AssetDB
{
    public partial class Attachment
    {
        public int SimCardId { get; set; }
        public int AssetId { get; set; }
        public int AppId { get; set; }
        public DateTime AttachmentTime { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }

        public virtual App App { get; set; }
        public virtual SimCard SimCard { get; set; }
    }
}
