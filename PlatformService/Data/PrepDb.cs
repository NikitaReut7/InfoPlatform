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



		if(!context.Companies.Any())
		{
            Console.WriteLine("--> Seeding Data...");

            context.Companies.AddRange(
                new Company()
                {
                    Name = "Microsoft",
					ExternalId = 1
				},
                new Company()
                {
                    Name = "Google LLS",
					ExternalId = 2
				}
            );

			context.SaveChanges();

            context.Platforms.AddRange(
				new Platform() { CompanyId = 1, Name = "Dot Net", Cost = "Free" },
                new Platform() { CompanyId = 1, Name = "SQL Server", Cost = "Free" },
                new Platform() { CompanyId = 2, Name = "Kubernetes", Cost = "Free" }
            );

			context.SaveChanges();
        }
        else
		{
			Console.WriteLine("--> We already have data.");
		}
	}
}

