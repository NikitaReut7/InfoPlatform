using CompanyService.Models;

namespace CompanyService.Data;
public interface ICompanyRepository
{
    bool SaveChanges();

    IEnumerable<Company> GetCompanies();

    Company GetCompanyById(int id);

    void CreateCompany(Company company);

    void DeteleCompany(Company company);
}

