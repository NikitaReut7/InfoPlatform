using Info.PlatformService.Data;
using Info.PlatformService.Data.PlatformRepository.BaseRepository;
using Info.PlatformService.Models;


namespace Info.PlatformService.Data.PlatformRepository;

public class PlatformRepository : PlatformRepositoryBase<Platform>, IPlatformRepository
{
    public PlatformRepository(AppDbContext dbContext) : base(dbContext)
    {

    }
}
