using MassTransit;
using Info.PlatformService.Data;
using Info.PlatformService.Models;
using Info.CompanyContracts;
using Info.PlatformService.Data.CompanyRepository;

namespace Info.PlatformService.AsyncDataServices.Consumers
{
    public class CompanyUpdatedConsumer : IConsumer<CompanyUpdated>
    {
        private readonly ICompanyRepository _repository;

        public CompanyUpdatedConsumer(ICompanyRepository repository)
        {
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<CompanyUpdated> context)
        {
            Console.WriteLine("Hit CompanyConsumer: updating company...");

            var message = context.Message;

            var item = _repository.Get(c => c.ExternalId == message.Id);

            if (item == null)
            {
                return;
            }

            item.Name = message.Name;

            _repository.Update(item);

            _repository.SaveChanges();
        }
    }
}