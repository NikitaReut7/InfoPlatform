using Info.CompanyService.DTOs;

namespace Info.CompanyService.SyncDataServices.Http;
public interface IPlatformDataClient
{
    Task SendCompanyToPlatform(CompanyReadDto company);
}

