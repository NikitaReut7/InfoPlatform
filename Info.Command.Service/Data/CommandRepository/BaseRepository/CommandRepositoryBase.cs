using Info.Common.Repository;
using Info.CommandService.Models;

namespace Info.CommandService.Data.CommandRepository.BaseRepository;

public class CommandRepositoryBase<T> : Repository<T>, ICommandRepositoryBase<T> where T : EntityBase
{
    public CommandRepositoryBase(AppDbContext dbContext) : base(dbContext)
    {

    }
}



