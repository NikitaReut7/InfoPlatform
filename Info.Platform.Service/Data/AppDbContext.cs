using Microsoft.EntityFrameworkCore;
using Info.PlatformService.Models;

namespace Info.PlatformService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
    {
        
    }

    public DbSet<Platform> Platforms {get; set;}

    public DbSet<Company> Companies { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder
			.Entity<Company>()
			.HasMany(p => p.Platforms)
			.WithOne(p => p.Company)
			.HasForeignKey(p => p.CompanyId);

       modelBuilder
			.Entity<Platform>()
			.HasOne(p => p.Company)
			.WithMany(p => p.Platforms)
			.HasForeignKey(p => p.CompanyId);
   }

}
