using System.Threading.Tasks;
using MassTransit;
using Info.CommandService.Data;
using Info.CompanyContracts;
using Info.PlatformContracts;

namespace Info.CommandService.AsyncDataServices.Consumers
{
    public class PlatformDeletedConsumer : IConsumer<PlatformDeleted>
    {
        private readonly ICommandRepository repository;

        public PlatformDeletedConsumer(ICommandRepository repository)
        {
            this.repository = repository;
        }

        public async Task Consume(ConsumeContext<PlatformDeleted> context)
        {
            var message = context.Message;

            var item = repository.GetPlatformByExternalId(message.Id);

            if (item == null)
            {
                return;
            }

            repository.DeletePlatform(item);
            repository.SaveChanges();
        }
    }
}