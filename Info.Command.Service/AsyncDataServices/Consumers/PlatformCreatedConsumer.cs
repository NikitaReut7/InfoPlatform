using Info.CommandService.Data.CommandRepository;
using Info.CommandService.Data.PlatformRepository;
using Info.CommandService.Models;
using Info.PlatformContracts;
using MassTransit;

namespace Info.CommandService.AsyncDataServices.Consumers
{
    public class PlatformCreatedConsumer : IConsumer<PlatformCreated>
    {
        private readonly IPlatformRepository _platformRepository;


        public PlatformCreatedConsumer(
            IPlatformRepository platformRepository
            )
        {
            _platformRepository = platformRepository;
        }

        public async Task Consume(ConsumeContext<PlatformCreated> context)
        {
            var message = context.Message;

            var isExist = _platformRepository.EntityExist(c=>c.ExternalId == message.Id);

            if (!isExist)
            {
                var platform = new Platform()
                {
                    ExternalId = message.Id,
                    Name = message.Name
                };

                _platformRepository.Create(platform);
                _platformRepository.SaveChanges();
            }
        }
    }
}