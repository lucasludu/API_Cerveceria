using Microsoft.AspNetCore.Identity;
using System;

namespace Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public required string Nombre { get; set; }
        public required string Apellido { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}
