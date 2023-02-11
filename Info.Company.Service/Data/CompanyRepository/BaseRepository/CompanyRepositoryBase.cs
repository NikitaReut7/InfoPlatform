using Info.Common.Repository;
using Info.CompanyService.Models;

namespace Info.CompanyService.Data.CompanyRepository.BaseRepository;

public class CompanyRepositoryBase<T> : Repository<T>, ICompanyRepositoryBase<T> where T : EntityBase
{
    public CompanyRepositoryBase(AppDbContext dbContext) : base(dbContext)
    {

    }
}



