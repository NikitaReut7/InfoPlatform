using Info.CommandService.Data;
using Info.CommandService.Models;
using Info.PlatformContracts;
using MassTransit;

namespace Info.CommandService.AsyncDataServices.Consumers
{
    public class PlatformCreatedConsumer : IConsumer<PlatformCreated>
    {
        private readonly ICommandRepository repository;

        public PlatformCreatedConsumer(ICommandRepository repository)
        {
            this.repository = repository;
        }

        public async Task Consume(ConsumeContext<PlatformCreated> context)
        {
            var message = context.Message;

            var isExist = repository.PlatformExistByExternalId(message.Id);

            if (isExist)
            {
                return;
            }

            var platform = new Platform()
            {
                ExternalId = message.Id,
                Name = message.Name
            };

            repository.CreatePlatform(platform);
            repository.SaveChanges();
        }
    }
}