using Microsoft.EntityFrameworkCore;
using CompanyService.Models;

namespace CompanyService.Data;
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
				new Company() { 
					Name = "Microsoft", 
					County = "USA", 
					Description = "American multinational technology corporation producing computer software, consumer electronics, personal computers, and related services" 
				},
                new Company() { 
					Name = "Google LLS", 
					County = "USA", 
					Description = "American multinational technology company focusing on search engine technology, online advertising, cloud computing, computer software, quantum computing, e-commerce, artificial intelligence,[9] and consumer electronics." 
				}
            );


			context.SaveChanges();
        }
        else
		{
			Console.WriteLine("--> We already have data.");
		}
	}
}

