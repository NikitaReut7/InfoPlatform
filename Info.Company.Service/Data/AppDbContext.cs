using Info.CompanyService.Models;
using Microsoft.EntityFrameworkCore;

namespace Info.CompanyService.Data;

public class AppDbContext : DbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
	{
	
	}

	public DbSet<Company> Companies { get; set; }
}