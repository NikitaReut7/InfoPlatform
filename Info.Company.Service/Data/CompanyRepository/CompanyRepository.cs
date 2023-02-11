using Info.CompanyService.Data;
using Info.CompanyService.Data.CompanyRepository.BaseRepository;
using Info.CompanyService.Models;


namespace Info.CompanyService.Data.CompanyRepository;

public class CompanyRepository : CompanyRepositoryBase<Company>, ICompanyRepository
{
    public CompanyRepository(AppDbContext dbContext) : base(dbContext)
    {

    }
}
