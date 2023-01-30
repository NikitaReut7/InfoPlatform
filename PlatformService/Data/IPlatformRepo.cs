using Microsoft.EntityFrameworkCore;
using PlatformService.Models;
using System;

namespace PlatformService.Data;
public interface IPlatformRepo
{
    bool SaveChanges();

    // Platforms

    IEnumerable<Platform> GetPlatforms();

    Platform GetPlatformById(int id);

    void CreatePlatform(int companyId, Platform platform);

    IEnumerable<Platform> GetPlatformsForCompany(int companyId);

    Platform GetPlatformForCompany(int companyId, int platformId);

    // Companies
    IEnumerable<Company> GetCompanies();

    void CreateCompany(Company company);

    void DeleteCompany(Company company);


    bool CompanyExist(int companyId);

    bool ExternalCompanyExist(int externalCompanyId);

    Company GetCompanyByExternalId(int externalId);
}
