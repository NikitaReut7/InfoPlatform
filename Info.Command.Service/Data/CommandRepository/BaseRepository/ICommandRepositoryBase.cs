using Info.Common.Repository;

namespace Info.CommandService.Data.CommandRepository.BaseRepository;

public interface ICommandRepositoryBase<T> : IRepository<T> where T : EntityBase
{

}
