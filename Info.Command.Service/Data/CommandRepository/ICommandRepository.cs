using Info.CommandService.Data.CommandRepository.BaseRepository;
using Info.CommandService.Models;

namespace Info.CommandService.Data.CommandRepository;

public interface ICommandRepository : ICommandRepositoryBase<Command>
{
}
