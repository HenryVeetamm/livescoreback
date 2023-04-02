using DAL;
using Domain;
using Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API;

public class AppDataHelper
{
    public static void SetUpAppData(IApplicationBuilder app, IConfiguration configuration)
    {
        var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();

        var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

        if (context == null)
        {
            throw new ApplicationException("Problem in services, No db context");
        }

        if (configuration.GetValue<bool>("DataInitialization:All"))
        {
            context.Database.EnsureDeleted();
            context.Database.Migrate();
        }

        if (configuration.GetValue<bool>("DataInitialization:MigrateDatabase"))
        {
            context.Database.Migrate();
        }

        if (configuration.GetValue<bool>("DataInitialization:SeedIdentity"))
        {
            var userManager = serviceScope.ServiceProvider.GetService<UserManager<AppUser>>();
            var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<AppRole>>();

            var roles = new string[] { "admin", "user" };

            foreach (var roleName in roles)
            {
                var role = roleManager.FindByNameAsync(roleName).Result;
                if (role == null)
                {
                    role = new AppRole()
                    {
                        Name = roleName
                    };

                    var result = roleManager.CreateAsync(role).Result;
                }
            }
        }

        if (configuration.GetValue<bool>("DataInitialization:SeedAdmin"))
        {
            var userManager = serviceScope.ServiceProvider.GetService<UserManager<AppUser>>();
            var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<AppRole>>();
            
            var existingUser =  userManager.FindByEmailAsync("admin@admin.com").Result;
            if (existingUser != null) return;
            
            var role = roleManager.FindByNameAsync("admin").Result;
            if (role == null)
            {
                role = new AppRole()
                {
                    Name = "admin"
                };

                var result = roleManager.CreateAsync(role).Result;
            }
        
            var user = new AppUser()
            {
                FirstName = "admin",
                LastName = "admin",
                UserName = "admin@admin.com",
                Email = "admin@admin.com",
                IsActive = true
            };
            
            var userResult = userManager.CreateAsync(user, "password").Result;
            var roleResult = userManager.AddToRoleAsync(user, "admin").Result;
        }
    }
}