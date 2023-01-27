using PlatformService.Models;

namespace PlatformService.Data;
public class PlatformRepo : IPlatformRepo
{
    private readonly AppDbContext _dbContext;
    public PlatformRepo(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void CreatePlatform(Platform platform)
    {
        if(platform == null)
        {
            throw new ArgumentNullException(nameof(platform));
        }

        _dbContext.Platforms.Add(platform);
    }

    public Platform GetPlatformById(int id)
    {
        return _dbContext.Platforms.FirstOrDefault(c => c.Id == id);
    }

    public IEnumerable<Platform> GetPlatforms()
    {
        return _dbContext.Platforms.ToList();
    }

    public bool SaveChanges()
    {
        return (_dbContext.SaveChanges() >= 0);
    }
}

