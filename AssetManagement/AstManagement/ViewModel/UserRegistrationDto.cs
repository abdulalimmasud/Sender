using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AstManagement.ViewModel
{
    public class UserRegistrationDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Username { get; set; }
        public string Email { get; set; }
        [Required, MinLength(6)]
        public string Password { get; set; }
    }
}
