using Info.CommandService.Data;
using Info.CommandService.Data.PlatformRepository.BaseRepository;
using Info.CommandService.Models;


namespace Info.CommandService.Data.PlatformRepository;

public class PlatformRepository : PlatformRepositoryBase<Platform>, IPlatformRepository
{
    public PlatformRepository(AppDbContext dbContext) : base(dbContext)
    {

    }
}
