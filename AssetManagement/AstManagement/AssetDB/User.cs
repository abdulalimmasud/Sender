using System;
using System.Collections.Generic;

#nullable disable

namespace AstManagement.AssetDB
{
    public partial class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public int IsActive { get; set; }
        public int IsDeleted { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
