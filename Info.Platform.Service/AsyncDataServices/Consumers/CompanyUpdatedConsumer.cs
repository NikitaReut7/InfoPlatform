using MassTransit;
using Info.PlatformService.Data;
using Info.PlatformService.Models;
using Info.CompanyContracts;

namespace Info.PlatformService.AsyncDataServices.Consumers
{
    public class CompanyUpdatedConsumer : IConsumer<CompanyUpdated>
    {
        private readonly IPlatformRepo repository;

        public CompanyUpdatedConsumer(IPlatformRepo repository)
        {
            this.repository = repository;
        }

        public async Task Consume(ConsumeContext<CompanyUpdated> context)
        {
            Console.WriteLine("Hit CompanyConsumer: updating company...");

            var message = context.Message;

            var item = repository.GetCompanyByExternalId(message.Id);

            if (item == null)
            {
                return;
            }

            item.Name = message.Name;

            repository.UpdateCompany(item);

            repository.SaveChanges();
        }
    }
}