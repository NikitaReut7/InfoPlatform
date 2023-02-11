using MassTransit;
using Info.CompanyContracts;
using Info.PlatformContracts;
using Info.PlatformService.Data.CompanyRepository;
using Info.PlatformService.Data.PlatformRepository;

namespace Info.PlatformService.AsyncDataServices.Consumers
{
    public class CompanyDeletedConsumer : IConsumer<CompanyDeleted>
    {
        private readonly ICompanyRepository _companyRepository;

        private readonly IPlatformRepository _platformRepository;

        private readonly IPublishEndpoint _publishEndpoint;


        public CompanyDeletedConsumer(
            ICompanyRepository companyRepository,
            IPlatformRepository platformRepository,
            IPublishEndpoint publishEndpoint)
        {
            _companyRepository = companyRepository;
            _platformRepository = platformRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<CompanyDeleted> context)
        {

            Console.WriteLine("Hit CompanyConsumer: deleting company...");

            var message = context.Message;

            var item = _companyRepository.Get(c => c.ExternalId == message.Id);

            var platforms = _platformRepository.GetAll(c => c.CompanyId == message.Id);

            if (item != null)
            {
                foreach (var platform in platforms)
                {
                    await _publishEndpoint.Publish(new PlatformDeleted(platform.Id));
                }

                _companyRepository.Remove(item);
                _companyRepository.SaveChanges();
            }
        }
    }
}