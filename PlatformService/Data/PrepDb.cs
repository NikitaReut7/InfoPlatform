using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data;
public static class PrepDb
{
	public static void PrepPopulation(IApplicationBuilder app, bool isProduction)
	{
		using(var serviceScoped = app.ApplicationServices.CreateScope()) 
		{
			SeedData(serviceScoped.ServiceProvider.GetService<AppDbContext>(),isProduction);
		}
	}

	private static void SeedData(AppDbContext context, bool isProduction)
	{
		if(isProduction)
		{
			Console.WriteLine("--> Attemting to apply migrations...");
			try
			{
				context.Database.Migrate();
			}
			catch(Exception	ex)
			{
				Console.WriteLine($"--> Could not run migrations: {ex.Message}");
			}
		}

		if(!context.Platforms.Any())
		{
            Console.WriteLine("--> Seeding Data...");

			context.Platforms.AddRange(
				new Platform() { Name = "Dot Net", Publisher = "Mucrosoft", Cost = "Free" },
                new Platform() { Name = "SQL Server", Publisher = "Mucrosoft", Cost = "Free" },
                new Platform() { Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost = "Free" }
            );

			context.SaveChanges();
        }
        else
		{
			Console.WriteLine("--> We already have data.");
		}
	}
}

