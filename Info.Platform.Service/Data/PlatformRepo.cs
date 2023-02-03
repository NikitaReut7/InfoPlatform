using Info.PlatformService.Models;

namespace Info.PlatformService.Data;
public class PlatformRepo : IPlatformRepo
{
    private readonly AppDbContext _dbContext;
    public PlatformRepo(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool CompanyExist(int companyId)
    {
        return _dbContext.Companies.Any(c => c.Id == companyId);
    }

    public void CreateCompany(Company company)
    {
        if (company == null)
        {
            throw new ArgumentNullException(nameof(company));
        }

        _dbContext.Companies.Add(company);
    }

    public void UpdateCompany(Company company)
    {
        if (company == null)
        {
            throw new ArgumentNullException(nameof(company));
        }

        _dbContext.Companies.Update(company);
    }


    public void DeleteCompany(Company company)
    {
        if (company == null)
        {
            throw new ArgumentNullException(nameof(company));
        }

        _dbContext.Companies.Remove(company);
    }

    public void CreatePlatform(int companyId, Platform platform)
    {
        if (platform == null)
        {
            throw new ArgumentNullException(nameof(platform));
        }

        platform.CompanyId = companyId;
        _dbContext.Platforms.Add(platform);
    }

    public void UpdatePlatform(Platform platform)
    {
        if (platform == null)
        {
            throw new ArgumentNullException(nameof(platform));
        }

        _dbContext.Platforms.Update(platform);
    }


    public void DeletePlatform(Platform platform)
    {
        if (platform == null)
        {
            throw new ArgumentNullException(nameof(platform));
        }

        _dbContext.Platforms.Remove(platform);
    }


    public bool ExternalCompanyExist(int externalCompanyId)
    {
        return _dbContext.Companies.Any(c => c.ExternalId == externalCompanyId);

    }

    public IEnumerable<Company> GetCompanies()
    {
        return _dbContext.Companies.ToList();
    }

    public Platform GetPlatformById(int id)
    {
        return _dbContext.Platforms.FirstOrDefault(c => c.Id == id);
    }

    public Company GetCompanyByExternalId(int externalId)
    {
        return _dbContext.Companies.FirstOrDefault(c => c.ExternalId == externalId);
    }

    public Platform GetPlatformForCompany(int companyId, int platformId)
    {
        return _dbContext.Platforms
            .FirstOrDefault(c => c.CompanyId == companyId && c.Id == platformId);
    }

    public IEnumerable<Platform> GetPlatforms()
    {
        return _dbContext.Platforms.ToList();
    }

    public IEnumerable<Platform> GetPlatformsForCompany(int companyId)
    {
        return _dbContext.Platforms
            .Where(c => c.CompanyId == companyId)
            .OrderBy(c => c.Company.Name);
    }

    public bool SaveChanges()
    {
        return (_dbContext.SaveChanges() >= 0);
    }
}

