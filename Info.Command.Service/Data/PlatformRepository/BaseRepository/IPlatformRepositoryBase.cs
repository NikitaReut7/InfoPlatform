using Info.Common.Repository;

namespace Info.CommandService.Data.PlatformRepository.BaseRepository;

public interface IPlatformRepositoryBase<T> : IRepository<T> where T : EntityBase
{

}
