using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public required string Nombre { get; set; }
        public required string Apellido { get; set; }
    }
}
