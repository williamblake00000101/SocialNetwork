using System.Reflection;
using System.Text.Json;
using DAL.Context;
using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class Seed
{
    public static void ContextSeed(ModelBuilder modelBuilder)
    {
        
        modelBuilder.Entity<Specialization>().HasData(
            new Specialization()
            {
                Id = 1, Name = "C# Developer"
            },
            new Specialization()
            {
                Id = 2, Name = "Java Developer"
            },
            new Specialization()
            {
                Id = 3, Name = "Python Developer"
            },
            new Specialization()
            {
                Id = 4, Name = "DevOps"
            }
        );
        
    }
    public static async Task SeedUsersAsync(UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager)
    {
        var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        
        if (await userManager.Users.AnyAsync()) return;

        var userData = await File.ReadAllTextAsync(path + @"/SeedData/UserSeedData.json");

        var options = new JsonSerializerOptions{PropertyNameCaseInsensitive = true};

        var users = JsonSerializer.Deserialize<List<AppUser>>(userData);

        var roles = new List<AppRole>
        {
            new AppRole{Name = "Member"},
            new AppRole{Name = "Administrator"},
            new AppRole{Name = "Moderator"},
        };

        foreach (var role in roles)
        {
            await roleManager.CreateAsync(role);
        }

        foreach (var user in users)
        {
            user.Photos.First().IsApproved = true;
            user.UserName = user.UserName.ToLower();
            await userManager.CreateAsync(user, "Pa$$w0rd");
            await userManager.AddToRoleAsync(user, "Member");
        }

        var admin = new AppUser
        {
            FirstName = "Admin",
            LastName = "Example",
            UserName = "admin",
            Email = "admin.example@outlook.com"
        };

        await userManager.CreateAsync(admin, "Pa$$w0rd!");
        await userManager.AddToRolesAsync(admin, new[] {"Administrator", "Moderator"});
    }
}