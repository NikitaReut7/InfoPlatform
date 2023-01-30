using CompanyService.DTOs;

namespace CompanyService.SyncDataServices.Http;
public interface IPlatformDataClient
{
    Task SendCompanyToPlatform(CompanyReadDto company);
}

