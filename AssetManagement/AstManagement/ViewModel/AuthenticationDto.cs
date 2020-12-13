using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AstManagement.ViewModel
{
    public class AuthenticationDto
    {
        [Required]
        public string Username { get; set; }
        [Required, MinLength(6)]
        public string Password { get; set; }
        public AuthenticationType GrantType { get; set; } = AuthenticationType.Password;
    }
    public enum AuthenticationType
    {
        Password = 1, RefreshToken = 2
    }
}
