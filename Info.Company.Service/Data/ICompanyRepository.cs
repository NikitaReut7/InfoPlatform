using Info.CompanyService.Models;

namespace Info.CompanyService.Data;
public interface ICompanyRepository
{
    bool SaveChanges();

    IEnumerable<Company> GetCompanies();

    Company GetCompanyById(int id);

    void CreateCompany(Company company);

    void DeteleCompany(Company company);

    void UpdateCompany(Company company);
}

