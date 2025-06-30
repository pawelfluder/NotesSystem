using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SharpIdentityProg.Data;
using SharpIdentityProg.Models;

namespace SharpIdentityProg.Services;

public class SeedData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        using var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        string[] roles = ["admin", "seller", "client"];

        foreach (var role in roles)
        {
            bool exists = await roleManager.RoleExistsAsync(role);
            if (!exists)
            {
                Console.WriteLine(role + " role is not defined and will be created");
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        var adminUsers = await userManager.GetUsersInRoleAsync("admin");
        if (adminUsers.Any())
        {
            return;
        }

        var adminUser = new ApplicationUser()
        {
            // FirstName = "Admin",
            // LastName = "Admin",
            Email = "admin@gmail.com",
            UserName = "admin@gmail.com", // UserName is used to authenticate the user
            // CreatedAt = DateTime.Now,
        };

        string initialPassword = "Pass001*";


        var result = await userManager.CreateAsync(adminUser, initialPassword);
        if (result.Succeeded)
        {
            // set the user role
            await userManager.AddToRoleAsync(adminUser, "admin");
            Console.WriteLine("Admin user created successfully");
            Console.WriteLine("Email: " + adminUser.Email);
            Console.WriteLine("Initial password: " + initialPassword);
        }
        else
        {
            Console.WriteLine("Failed to create Admin user!");
        }
    }
}