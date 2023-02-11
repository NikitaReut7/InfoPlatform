using Info.Common.Repository;
using Info.PlatformService.Models;

namespace Info.PlatformService.Data.PlatformRepository.BaseRepository;

public class PlatformRepositoryBase<T> : Repository<T>, IPlatformRepositoryBase<T> where T : EntityBase
{
    public PlatformRepositoryBase(AppDbContext dbContext) : base(dbContext)
    {

    }
}



