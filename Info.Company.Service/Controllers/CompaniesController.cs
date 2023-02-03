using AutoMapper;
using Info.CompanyService.Data;
using Info.CompanyService.DTOs;
using Info.CompanyService.Models;
using Info.CompanyService.SyncDataServices.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MassTransit.Transports;
using MassTransit;
using Info.CompanyContracts;

namespace Info.CompanyService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyRepository _repository;
        private readonly IMapper _mapper;
        private readonly IPlatformDataClient _platformDataClient;
        private readonly IPublishEndpoint _publishEndpoint;

        public CompaniesController(
            ICompanyRepository repository,
            IMapper mapper,
            IPlatformDataClient platformDataClient,
            IPublishEndpoint publishEndpoint)
        {
            _repository = repository;
            _mapper = mapper;
            _platformDataClient = platformDataClient;
            _publishEndpoint = publishEndpoint;
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
                await _publishEndpoint.Publish(new CompanyCreated(companyReadDto.Id, companyReadDto.Name));

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

        [HttpPut("{id}", Name = "UpdateCompany")]
        public async Task<ActionResult> UpdateCompany(int id, CompanyUpdateDto companyUpdateDto)
        {
            Console.WriteLine("--> Updating company...");

            var company = _repository.GetCompanyById(id);

            if (company == null)
            {
                return NotFound();
            }

            company.Name = companyUpdateDto.Name;
            company.County = companyUpdateDto.County ?? company.County;
            company.Description = companyUpdateDto.Description ?? company.Description;

            _repository.UpdateCompany(company);
            _repository.SaveChanges();

            var companyReadDto = _mapper.Map<CompanyUpdateDto>(company);

            try
            {
                await _publishEndpoint.Publish(new CompanyUpdated(id, companyReadDto.Name));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send asynchronously: {ex.Message}");
            }


            return Ok(companyReadDto);
        }

        [HttpDelete("{id}", Name = "DeleteCompany")]
        public async Task<ActionResult> DeleteCompany(int id)
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
                    await _publishEndpoint.Publish(new CompanyDeleted(id));
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
