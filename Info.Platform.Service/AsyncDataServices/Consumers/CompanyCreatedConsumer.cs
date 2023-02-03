using System.Threading.Tasks;
using Info.PlatformService.AsyncDataServices;
using Info.PlatformService.Data;
using Info.PlatformService.DTOs;
using Info.PlatformService.Models;
using Info.CompanyContracts;
using MassTransit;

namespace Info.PlatformService.AsyncDataServices.Consumers
{
    public class CompanyCreatedConsumer : IConsumer<CompanyCreated>
    {
        private readonly IPlatformRepo repository;

        public CompanyCreatedConsumer(IPlatformRepo repository)
        {
            this.repository = repository;
        }

        public async Task Consume(ConsumeContext<CompanyCreated> context)
        {
            Console.WriteLine("Hit CompanyConsumer: creating company...");
            var message = context.Message;

            var isExist = repository.ExternalCompanyExist(message.Id);

            if (isExist)
            {
                return;
            }

            var company = new Company()
            {
                ExternalId = message.Id,
                Name = message.Name
            };

            repository.CreateCompany(company);
            repository.SaveChanges();
        }
    }
}