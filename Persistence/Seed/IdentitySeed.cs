using Application.Constants;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Persistence.Seed
{
    public static class IdentitySeed
    {
        public static async Task SeedAsync (IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            foreach (var role in RolesConstants.ValidRoles)
            {
                if(!await roleManager.RoleExistsAsync (role))
                    await roleManager.CreateAsync(new ApplicationRole { Name = role });
            }

            var adminEmail = "admin@cerveceria.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                var user = new ApplicationUser { UserName = adminEmail, Nombre = "Admin", Apellido = "Cervecero", Email = adminEmail };
                await userManager.CreateAsync(user, "Admin123!");
                await userManager.AddToRoleAsync(user, RolesConstants.Admin);
            }

            var brewerEmail = "brewer@cerveceria.com";
            var brewerUser = await userManager.FindByEmailAsync(brewerEmail);
            if (brewerUser == null)
            {
                var user = new ApplicationUser { UserName = brewerEmail, Nombre = "Maestro", Apellido = "Cervecero", Email = brewerEmail };
                await userManager.CreateAsync(user, "Brewer123!");
                await userManager.AddToRoleAsync(user, RolesConstants.Brewery);
            }

            var wholesalerEmail = "wholesaler@cerveceria.com";
            var wholesalerUser = await userManager.FindByEmailAsync(wholesalerEmail);
            if (wholesalerUser == null)
            {
                var user = new ApplicationUser { UserName = wholesalerEmail, Nombre = "Gran", Apellido = "Mayorista", Email = wholesalerEmail };
                await userManager.CreateAsync(user, "Wholesaler123!");
                await userManager.AddToRoleAsync(user, RolesConstants.Wholesaler);
            }
        }
    }
}
