using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Info.PlatformService.Data;
using Info.PlatformService.DTOs;
using Info.PlatformService.Models;
using System.Text.Json;
using Info.PlatformService.Data.CompanyRepository;

namespace Info.PlatformService.Controllers
{
    [Route("api/p/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyRepository _repository;
        private readonly IMapper _mapper;

        public CompaniesController(
            ICompanyRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CompanyReadDto>> GetPlatforms()
        {
            Console.WriteLine("--> Getting Companies from Company Service");

            var companies = _repository.GetAll();

            var companiesReadDtos = _mapper.Map<IEnumerable<CompanyReadDto>>(companies);

            return Ok(companiesReadDtos);
        }

        [HttpPost]
        public ActionResult TetsInboundConnection(object obj)
        {
            Console.WriteLine("--> Inbound post # Platform Service");

            try
            {
                var companyPublishedDto = JsonSerializer.Deserialize<CompanyPublishedDto>(obj.ToString());
                var company = _mapper.Map<Company>(companyPublishedDto);

                if (!_repository.EntityExist(c => c.ExternalId == company.ExternalId))
                {
                    _repository.Create(company);
                    _repository.SaveChanges();
                    Console.WriteLine("--> Company added!");

                }
                else
                {
                    Console.WriteLine("--> Company already exists...");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not add company to DB : {ex.Message}");
            }

            return Ok("Inbound test of from Companies Controller");
        }
    }
}
