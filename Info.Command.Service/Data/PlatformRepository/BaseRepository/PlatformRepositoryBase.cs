using Info.Common.Repository;
using Info.CommandService.Models;

namespace Info.CommandService.Data.PlatformRepository.BaseRepository;

public class PlatformRepositoryBase<T> : Repository<T>, IPlatformRepositoryBase<T> where T : EntityBase
{
    public PlatformRepositoryBase(AppDbContext dbContext) : base(dbContext)
    {

    }
}



