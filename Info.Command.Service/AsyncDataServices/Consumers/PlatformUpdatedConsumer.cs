using MassTransit;
using Info.CommandService.Data;
using Info.CommandService.Models;
using Info.CompanyContracts;
using Info.PlatformContracts;

namespace Info.CommandService.AsyncDataServices.Consumers
{
    public class PlatformUpdatedConsumer : IConsumer<PlatformUpdated>
    {
        private readonly ICommandRepository repository;

        public PlatformUpdatedConsumer(ICommandRepository repository)
        {
            this.repository = repository;
        }

        public async Task Consume(ConsumeContext<PlatformUpdated> context)
        {
            var message = context.Message;

            var item = repository.GetPlatformByExternalId(message.Id);

            if (item == null)
            {
                return;
            }

            item.Name = message.Name;

            repository.UpdatePlatform(item);

            repository.SaveChanges();
        }
    }
}