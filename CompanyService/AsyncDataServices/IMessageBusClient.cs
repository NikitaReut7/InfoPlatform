using CompanyService.DTOs;

namespace CompanyService.AsyncDataServices;
public interface IMessageBusClient
{
    void PublishNewCompany(CompanyPublishedDto companyPublishedDto);

    void DeleteCompany(CompanyDeletedDto companyDeletedDto);

}

