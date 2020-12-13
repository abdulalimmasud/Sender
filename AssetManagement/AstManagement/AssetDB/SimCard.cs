using System;
using System.Collections.Generic;

#nullable disable

namespace AstManagement.AssetDB
{
    public partial class SimCard
    {
        public int Id { get; set; }
        public string MobileNumber { get; set; }
        public string SimNumber { get; set; }
        public int OperatorId { get; set; }
        public int OperatorPackageId { get; set; }
        public int IsActive { get; set; }
        public int IsDeleted { get; set; }
        public DateTime CreationTime { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }
        public DateTime UpdateTime { get; set; }
        public string UpdateByUser { get; set; }
        public int UpdateBy { get; set; }

        public virtual MobileOperator Operator { get; set; }
        public virtual MobileOperatorPackage OperatorPackage { get; set; }
        public virtual Attachment Attachment { get; set; }
    }
}
