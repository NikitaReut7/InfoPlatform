using Microsoft.EntityFrameworkCore;
using Info.CommandService.Models;
using Info.CommandService.SyncDataServices.Grps;
using Info.CommandService.Data.PlatformRepository;

namespace Info.CommandService.Data;
public static class PrepDb
{
    public static void PrepPopulation(IApplicationBuilder app, bool isProduction)
    {
        using (var serviceScoped = app.ApplicationServices.CreateScope())
        {
            var grpcClient = serviceScoped.ServiceProvider.GetService<IPlatformDataClient>();

            var platforms = grpcClient.ReturnAllPlatforms();

            SeedData(
                serviceScoped.ServiceProvider.GetService<AppDbContext>(),
                serviceScoped.ServiceProvider.GetService<IPlatformRepository>(),
                platforms,
                isProduction);
        }
    }

    private static void SeedData(
        AppDbContext context,
        IPlatformRepository repository,
        IEnumerable<Platform> platforms,
        bool isProduction)
    {
        if (isProduction)
        {
            Console.WriteLine("--> Attemting to apply migrations...");
            try
            {
                context.Database.Migrate();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not run migrations: {ex.Message}");
            }
        }

        if (platforms.Any())
        {
            Console.WriteLine("--> Seeding Platforms from GRPS Service...");

            foreach (var platform in platforms)
            {
                if (!repository.EntityExist(c=> c.ExternalId == platform.ExternalId))
                {
                    repository.Create(platform);
                    repository.SaveChanges();

                    Console.WriteLine("--> New platform added from GRPS Service!");
                }
            }

            // context.Platforms.AddRange(
            // 	new Platform() { Name = "Dot Net", ExternalId = 1 },
            // 	new Platform() { Name = "SQL Server", ExternalId = 1 });

            // context.SaveChanges();
        }
        // if(!context.Commands.Any())
        // {
        //     Console.WriteLine("--> Seeding Commands...");

        //     context.Commands.AddRange(
        // 		new Command() { HowTo = "Build .NET App", CommandLine = "dotnet build", PlatformId = 1},
        //         new Command() { HowTo = "Run .NET App", CommandLine = "dotnet run", PlatformId = 1 }
        // 		);

        // 	context.SaveChanges();
        // }
        // else
        // {
        // 	Console.WriteLine("--> We already have commands.");
        // }
    }
}

