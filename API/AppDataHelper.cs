﻿using DAL;
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
    }
}