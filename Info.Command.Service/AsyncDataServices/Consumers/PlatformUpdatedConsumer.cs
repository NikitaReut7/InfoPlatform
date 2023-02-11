using MassTransit;
using Info.CommandService.Data;
using Info.CommandService.Models;
using Info.CompanyContracts;
using Info.PlatformContracts;
using Info.CommandService.Data.PlatformRepository;

namespace Info.CommandService.AsyncDataServices.Consumers
{
    public class PlatformUpdatedConsumer : IConsumer<PlatformUpdated>
    {
        private readonly IPlatformRepository _platformRepository;

        public PlatformUpdatedConsumer(IPlatformRepository platformRepository)
        {
            _platformRepository = platformRepository;
        }

        public async Task Consume(ConsumeContext<PlatformUpdated> context)
        {
            var message = context.Message;

            var item = _platformRepository.Get(c => c.ExternalId == message.Id);

            if (item != null)
            {
                item.Name = message.Name;

                _platformRepository.Update(item);

                _platformRepository.SaveChanges();
            }
        }
    }
}