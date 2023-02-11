using Info.Common.Repository;

namespace Info.PlatformService.Data.CompanyRepository.BaseRepository;

public interface ICompanyRepositoryBase<T> : IRepository<T> where T : EntityBase
{

}
