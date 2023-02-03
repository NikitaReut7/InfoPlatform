using System.Threading.Tasks;
using MassTransit;
using Info.PlatformService.Data;
using Info.CompanyContracts;
using Info.PlatformContracts;

namespace Info.PlatformService.AsyncDataServices.Consumers
{
    public class CompanyDeletedConsumer : IConsumer<CompanyDeleted>
    {
        private readonly IPlatformRepo repository;
        private readonly IPublishEndpoint _publishEndpoint;


        public CompanyDeletedConsumer(IPlatformRepo repository, IPublishEndpoint publishEndpoint)
        {
            this.repository = repository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<CompanyDeleted> context)
        {

            Console.WriteLine("Hit CompanyConsumer: deleting company...");

            var message = context.Message;

            var item = repository.GetCompanyByExternalId(message.Id);

            var platforms = repository.GetPlatformsForCompany(message.Id);

            if (item == null)
            {
                return;
            }

            foreach (var platform in platforms)
            {
                await _publishEndpoint.Publish(new PlatformDeleted(platform.Id));
            }

            repository.DeleteCompany(item);
            repository.SaveChanges();
        }
    }
}