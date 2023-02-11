using Info.Common.Repository;

namespace Info.PlatformService.Data.PlatformRepository.BaseRepository;

public interface IPlatformRepositoryBase<T> : IRepository<T> where T : EntityBase
{

}
