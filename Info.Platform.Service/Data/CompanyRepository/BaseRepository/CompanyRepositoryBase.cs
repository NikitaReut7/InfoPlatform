using Info.Common.Repository;
using Info.PlatformService.Models;

namespace Info.PlatformService.Data.CompanyRepository.BaseRepository;

public class CompanyRepositoryBase<T> : Repository<T>, ICompanyRepositoryBase<T> where T : EntityBase
{
    public CompanyRepositoryBase(AppDbContext dbContext) : base(dbContext)
    {

    }
}



