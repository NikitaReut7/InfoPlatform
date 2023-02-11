using Info.Common.Repository;

namespace Info.CompanyService.Data.CompanyRepository.BaseRepository;

public interface ICompanyRepositoryBase<T> : IRepository<T> where T : EntityBase
{

}
