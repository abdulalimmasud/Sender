using System;
using System.Collections.Generic;

#nullable disable

namespace AstManagement.AssetDB
{
    public partial class AttachmentLog
    {
        public int Id { get; set; }
        public int SimCardId { get; set; }
        public int AssetId { get; set; }
        public int AppId { get; set; }
        public string UserName { get; set; }
        public int LogType { get; set; }
        public DateTime LogTime { get; set; }
        public int UserId { get; set; }

        public virtual App App { get; set; }
    }
}
