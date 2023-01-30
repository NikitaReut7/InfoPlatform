using AutoMapper;
using PlatformService.Data;
using PlatformService.DTOs;
using PlatformService.Models;
using System.Text.Json;

namespace PlatformService.EventProcessing;
public class EventProcessor : IEventProcessor
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IMapper _mapper;

    public EventProcessor(
        IServiceScopeFactory serviceScopeFactory,
        IMapper mapper)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _mapper = mapper;
    }

    public void ProcessEvent(string message)
    {
        var eventType = DetermineEvent(message);

        switch (eventType)
        {
            case EventType.CompanyPublished:
                AddCompany(message);
                break;
            case EventType.CompanyDeleted:
                DeleteCompany(message);
                break;
            default:
                break;
        }
    }

    private EventType DetermineEvent(string notificationMessage)
    {
        Console.WriteLine("--> Determining Event");

        var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);

        switch (eventType?.Event)
        {
            case "Company_Published":
                Console.WriteLine("--> Company Published Event Detected");
                return EventType.CompanyPublished;
            case "Company_Deleted":
                Console.WriteLine("--> Company Deleted Event Detected");
                return EventType.CompanyDeleted;
            default:
                Console.WriteLine("--> Could not determine the event type");
                return EventType.Undetermined;
        }
    }

    private void AddCompany(string companyPublishedMessage)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var repository = scope.ServiceProvider.GetRequiredService<IPlatformRepo>();

            var companyPublishedDto = JsonSerializer.Deserialize<CompanyPublishedDto>(companyPublishedMessage);

            try
            {
                var company = _mapper.Map<Company>(companyPublishedDto);

                if (!repository.ExternalCompanyExist(company.ExternalId))
                {
                    repository.CreateCompany(company);
                    repository.SaveChanges();
                    Console.WriteLine("--> Company added!");

                }
                else
                {
                    Console.WriteLine("--> Company already exists...");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not add company to DB : {ex.Message}");
            }
        }
    }

    private void DeleteCompany(string companyDeletedMessage)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var repository = scope.ServiceProvider.GetRequiredService<IPlatformRepo>();

            var companyDeletedDto = JsonSerializer.Deserialize<CompanyDeletedDto>(companyDeletedMessage);

            try
            {
                var entity = _mapper.Map<Company>(companyDeletedDto);

                if (repository.ExternalCompanyExist(entity.ExternalId))
                {
                    var company = repository.GetCompanyByExternalId(entity.ExternalId);

                    repository.DeleteCompany(company);
                    repository.SaveChanges();
                    Console.WriteLine("--> Company deleted!");

                }
                else
                {
                    Console.WriteLine("--> Company doesn't exist...");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not delete company from DB : {ex.Message}");
            }
        }
    }

}

enum EventType
{
    CompanyPublished,
    CompanyDeleted,
    Undetermined
}