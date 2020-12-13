using System;
using System.Collections.Generic;

#nullable disable

namespace AstManagement.AssetDB
{
    public partial class MobileOperator
    {
        public MobileOperator()
        {
            MobileOperatorPackages = new HashSet<MobileOperatorPackage>();
            SimCards = new HashSet<SimCard>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int IsActive { get; set; }
        public int IsDeleted { get; set; }
        public DateTime CreationTime { get; set; }

        public virtual ICollection<MobileOperatorPackage> MobileOperatorPackages { get; set; }
        public virtual ICollection<SimCard> SimCards { get; set; }
    }
}
