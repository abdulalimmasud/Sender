using System;
using System.Collections.Generic;

#nullable disable

namespace AstManagement.AssetDB
{
    public partial class MobileOperatorPackage
    {
        public MobileOperatorPackage()
        {
            SimCards = new HashSet<SimCard>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int OperatorId { get; set; }
        public int IsActive { get; set; }
        public int IsDeleted { get; set; }
        public DateTime CreationTime { get; set; }

        public virtual MobileOperator Operator { get; set; }
        public virtual ICollection<SimCard> SimCards { get; set; }
    }
}
