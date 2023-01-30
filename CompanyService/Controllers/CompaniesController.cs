using AutoMapper;
using CompanyService.Data;
using CompanyService.DTOs;
using CompanyService.Models;
using CompanyService.SyncDataServices.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CompanyService.AsyncDataServices;

namespace CompanyService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyRepository _repository;
        private readonly IMapper _mapper;
        private readonly IPlatformDataClient _platformDataClient;
        private readonly IMessageBusClient _messageBusClient;

        public CompaniesController(
            ICompanyRepository repository,
            IMapper mapper,
            IPlatformDataClient platformDataClient,
            IMessageBusClient messageBusClient)
        {
            _repository = repository;
            _mapper = mapper;
            _platformDataClient = platformDataClient;
            _messageBusClient = messageBusClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CompanyReadDto>> GetCompanies()
        {
            Console.WriteLine("--> Getting Companies...");

            var companies = _repository.GetCompanies();

            return Ok(_mapper.Map<IEnumerable<CompanyReadDto>>(companies));
        }

        [HttpPost]
        public async Task<ActionResult<CompanyReadDto>> CreateCompany(CompanyCreateDto companyCreateDto)
        {
            Console.WriteLine("--> Creating company...");

            var company = _mapper.Map<Company>(companyCreateDto);

            _repository.CreateCompany(company);
            _repository.SaveChanges();

            var companyReadDto = _mapper.Map<CompanyReadDto>(company);

            //// Send Sync message
            //try
            //{
            //    await _platformDataClient.SendCompanyToPlatform(companyReadDto);

            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"--> Could not send synchronously: {ex.Message}");

            //}

            // Send Async Message
            try
            {
                var companyPublishedDto = _mapper.Map<CompanyPublishedDto>(companyReadDto);
                companyPublishedDto.Event = "Company_Published";
                _messageBusClient.PublishNewCompany(companyPublishedDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send asynchronously: {ex.Message}");
            }

            return CreatedAtRoute(nameof(GetCompanyById), new { Id = companyReadDto.Id }, companyReadDto);
        }

        [HttpGet("{id}", Name = "GetCompanyById")]
        public ActionResult<CompanyReadDto> GetCompanyById(int id)
        {
            Console.WriteLine("--> Getting Company by id...");

            var company = _repository.GetCompanyById(id);

            if (company != null)
            {
                return Ok(_mapper.Map<CompanyReadDto>(company));
            }

            return NotFound();
        }


        [HttpDelete("{id}", Name = "DeleteCompany")]
        public ActionResult DeleteCompany(int id)
        {

            Console.WriteLine("--> Deleting company...");

            var company = _repository.GetCompanyById(id);

            if (company != null)
            {

                var companyReadDto = _mapper.Map<CompanyReadDto>(company);

                _repository.DeteleCompany(company);
                _repository.SaveChanges();

                try
                {
                    var companyDeletedDto = _mapper.Map<CompanyDeletedDto>(companyReadDto);
                    companyDeletedDto.Event = "Company_Deleted";
                    _messageBusClient.DeleteCompany(companyDeletedDto);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not send asynchronously: {ex.Message}");
                }


                return NoContent();
            }

            return NotFound();
        }
    }
}
