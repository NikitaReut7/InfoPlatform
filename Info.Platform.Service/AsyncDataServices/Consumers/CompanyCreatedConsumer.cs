using System.Threading.Tasks;
using Info.PlatformService.AsyncDataServices;
using Info.PlatformService.Data;
using Info.PlatformService.DTOs;
using Info.PlatformService.Models;
using Info.CompanyContracts;
using MassTransit;
using Info.PlatformService.Data.CompanyRepository;

namespace Info.PlatformService.AsyncDataServices.Consumers
{
    public class CompanyCreatedConsumer : IConsumer<CompanyCreated>
    {
        private readonly ICompanyRepository _repository;

        public CompanyCreatedConsumer(ICompanyRepository repository)
        {
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<CompanyCreated> context)
        {
            Console.WriteLine("Hit CompanyConsumer: creating company...");
            var message = context.Message;

            var isExist = _repository.EntityExist(c => c.ExternalId == message.Id);

            if (!isExist)
            {
                var company = new Company()
                {
                    ExternalId = message.Id,
                    Name = message.Name
                };

                _repository.Create(company);
                _repository.SaveChanges();
            }
        }
    }
}