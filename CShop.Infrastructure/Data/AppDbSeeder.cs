using CShop.Application.Interfaces;
using CShop.Domain.Entities;
using CShop.Domain.Identity;
using CShop.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.Infrastructure.Data
{
    public static class AppDbSeeder
    {
        private static readonly string[] DefaultRoles = { "Admin", "Customer" };
        public static async Task SeedRolesAndAdminAsync(
            UserManager<AppUser> userManager, 
            RoleManager<AppRole> roleManager, 
            IConfiguration configuration,
            ILogger logger
        )
        {
            // Seed Roles
            foreach (var roleName in DefaultRoles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    var role = new AppRole { Name = roleName };
                    await roleManager.CreateAsync(role);
                    logger?.LogInformation($"Role '{roleName}' created.");
                }
            }
            // Seed Admin User
            var adminEmail = configuration["AdminUser:Email"];
            var adminPassword = configuration["AdminUser:Password"];

            if (string.IsNullOrWhiteSpace(adminEmail) || string.IsNullOrWhiteSpace(adminPassword))
            {
                logger?.LogWarning("AdminUser configuration is missing. Skipping admin user creation.");
                return;
            }

            // Check if admin user exists
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new AppUser
                {
                    UserName = "admin",
                    Email = adminEmail,
                    EmailConfirmed = true,
                    Profile = new UserProfile
                    {
                        FirstName = "Admin",
                        LastName = "User",
                    }
                };


                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    logger?.LogInformation($"Admin user '{adminEmail}' created.");
                }
                else
                {
                    logger?.LogError($"Failed to create admin: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    return;
                }


                // Ensure Admin Role
                if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                    logger?.LogInformation("Admin user assigned to Admin role.");
                }

                logger?.LogInformation("Seeding complete.");
            }
        }
    }
}
