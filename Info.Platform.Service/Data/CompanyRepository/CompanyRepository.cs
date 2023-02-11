using Info.PlatformService.Data;
using Info.PlatformService.Data.CompanyRepository.BaseRepository;
using Info.PlatformService.Models;


namespace Info.PlatformService.Data.CompanyRepository;

public class CompanyRepository : CompanyRepositoryBase<Company>, ICompanyRepository
{
    public CompanyRepository(AppDbContext dbContext) : base(dbContext)
    {

    }
}
