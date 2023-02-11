using Info.CommandService.Data;
using Info.CommandService.Data.CommandRepository.BaseRepository;
using Info.CommandService.Models;


namespace Info.CommandService.Data.CommandRepository;

public class CommandRepository : CommandRepositoryBase<Command>, ICommandRepository
{
    public CommandRepository(AppDbContext dbContext) : base(dbContext)
    {

    }
}
