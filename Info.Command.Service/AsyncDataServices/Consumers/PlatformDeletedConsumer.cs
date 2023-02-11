using MassTransit;
using Info.PlatformContracts;
using Info.CommandService.Data.PlatformRepository;

namespace Info.CommandService.AsyncDataServices.Consumers
{
    public class PlatformDeletedConsumer : IConsumer<PlatformDeleted>
    {
        private readonly IPlatformRepository _platformRepository;

        public PlatformDeletedConsumer(IPlatformRepository platformRepository)
        {
            _platformRepository = platformRepository;
        }

        public async Task Consume(ConsumeContext<PlatformDeleted> context)
        {
            var message = context.Message;

            var item = _platformRepository.Get(c => c.ExternalId == message.Id);

            if (item != null)
            {
                _platformRepository.Remove(item);
                _platformRepository.SaveChanges();
            }
        }
    }
}