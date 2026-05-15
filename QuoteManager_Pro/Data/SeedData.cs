using Microsoft.AspNetCore.Identity;
using QuoteManager_Pro.Models;

namespace QuoteManager_Pro.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            Console.WriteLine("🚀 SeedData.InitializeAsync started...");

            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Create roles
            Console.WriteLine("📋 Checking/Creating roles...");
            string[] roles = { "Admin", "Manager", "Client" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                    Console.WriteLine($"✅ Created role: {role}");
                }
                else
                {
                    Console.WriteLine($"✅ Role already exists: {role}");
                }
            }

            // Create admin user
            var adminEmail = "admin@quotemanager.com";
            Console.WriteLine($"🔍 Checking for admin user: {adminEmail}");
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                Console.WriteLine("📝 Admin user not found, creating...");
                var admin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "System",
                    LastName = "Administrator",
                    RegistrationDate = DateTime.UtcNow,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(admin, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                    Console.WriteLine("✅ Created admin user successfully");
                }
                else
                {
                    Console.WriteLine("❌ Failed to create admin user:");
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"   - {error.Description}");
                    }
                }
            }
            else
            {
                Console.WriteLine("✅ Admin user already exists");
                // Verify the user has the Admin role
                var existingAdmin = await userManager.FindByEmailAsync(adminEmail);
                if (!await userManager.IsInRoleAsync(existingAdmin, "Admin"))
                {
                    await userManager.AddToRoleAsync(existingAdmin, "Admin");
                    Console.WriteLine("✅ Added Admin role to existing admin user");
                }
            }

            // Create manager user
            var managerEmail = "manager@quotemanager.com";
            Console.WriteLine($"🔍 Checking for manager user: {managerEmail}");
            if (await userManager.FindByEmailAsync(managerEmail) == null)
            {
                Console.WriteLine("📝 Manager user not found, creating...");
                var manager = new ApplicationUser
                {
                    UserName = managerEmail,
                    Email = managerEmail,
                    FirstName = "Demo",
                    LastName = "Manager",
                    RegistrationDate = DateTime.UtcNow,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(manager, "Manager@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(manager, "Manager");
                    Console.WriteLine("✅ Created manager user successfully");
                }
                else
                {
                    Console.WriteLine("❌ Failed to create manager user:");
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"   - {error.Description}");
                    }
                }
            }
            else
            {
                Console.WriteLine("✅ Manager user already exists");
            }

            // Create demo client user
            var clientEmail = "client@quotemanager.com";
            Console.WriteLine($"🔍 Checking for client user: {clientEmail}");
            if (await userManager.FindByEmailAsync(clientEmail) == null)
            {
                Console.WriteLine("📝 Client user not found, creating...");
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
                {
                    await userManager.AddToRoleAsync(client, "Client");
                    Console.WriteLine("✅ Created client user successfully");
                }
                else
                {
                    Console.WriteLine("❌ Failed to create client user:");
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"   - {error.Description}");
                    }
                }
            }
            else
            {
                Console.WriteLine("✅ Client user already exists");
            }

            Console.WriteLine("🏁 SeedData.InitializeAsync completed!");
        }
    }
}