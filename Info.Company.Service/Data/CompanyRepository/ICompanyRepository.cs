using Info.CompanyService.Data.CompanyRepository.BaseRepository;
using Info.CompanyService.Models;

namespace Info.CompanyService.Data.CompanyRepository;

public interface ICompanyRepository : ICompanyRepositoryBase<Company>
{
}
