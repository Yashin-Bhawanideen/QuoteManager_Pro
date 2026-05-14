using Microsoft.AspNetCore.Identity;
using QuoteManager_Pro.Models;

namespace QuoteManager_Pro.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Create roles
            string[] roles = { "Admin", "Manager", "Client" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            // Create admin user
            var adminEmail = "admin@quotemanager.com";
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var admin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "System",
                    LastName = "Administrator",
                    RegistrationDate = DateTime.UtcNow,
                    EmailConfirmed = true  // Add this to skip email confirmation
                };

                var result = await userManager.CreateAsync(admin, "Admin@123");
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(admin, "Admin");
            }

            // Create manager user
            var managerEmail = "manager@quotemanager.com";
            if (await userManager.FindByEmailAsync(managerEmail) == null)
            {
                var manager = new ApplicationUser
                {
                    UserName = managerEmail,
                    Email = managerEmail,
                    FirstName = "Demo",
                    LastName = "Manager",
                    RegistrationDate = DateTime.UtcNow,
                    EmailConfirmed = true  // Add this to skip email confirmation
                };

                var result = await userManager.CreateAsync(manager, "Manager@123");
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(manager, "Manager");  // ← FIXED: "Manager" not "MAnager"
            }

            // Optional: Create a demo client user for testing
            var clientEmail = "client@quotemanager.com";
            if (await userManager.FindByEmailAsync(clientEmail) == null)
            {
                var client = new ApplicationUser
                {
                    UserName = clientEmail,
                    Email = clientEmail,
                    FirstName = "Test",
                    LastName = "Client",
                    RegistrationDate = DateTime.UtcNow,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(client, "Client@123");
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(client, "Client");
            }
        }
    }
}