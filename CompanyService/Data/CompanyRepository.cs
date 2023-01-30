using CompanyService.Models;
using Microsoft.EntityFrameworkCore;

namespace CompanyService.Data;
public class CompanyRepository : ICompanyRepository
{
    private readonly AppDbContext _context;

    public CompanyRepository(AppDbContext context)
    {
        _context = context;
    }

    public void CreateCompany(Company company)
    {
        if (company == null)
        {
            throw new ArgumentNullException(nameof(company));
        }

        _context.Companies.Add(company);
    }

    public IEnumerable<Company> GetCompanies()
    {
        return _context.Companies.ToList();
    }

    public Company GetCompanyById(int id)
    {
        return _context.Companies.FirstOrDefault(c => c.Id == id);
    }

    public void DeteleCompany(Company company)
    {
        _context.Companies.Remove(company);
    }

    public bool SaveChanges()
    {
        return (_context.SaveChanges() >= 0);
    }
}

