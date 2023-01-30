﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.DTOs;
using PlatformService.Models;
using System.Text.Json;

namespace PlatformService.Controllers
{
    [Route("api/p/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly IPlatformRepo _repository;
        private readonly IMapper _mapper;

        public CompaniesController(IPlatformRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CompanyReadDto>> GetPlatforms()
        {
            Console.WriteLine("--> Getting Companies from Company Service");

            var companies = _repository.GetCompanies();

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

                if (!_repository.ExternalCompanyExist(company.ExternalId))
                {
                    _repository.CreateCompany(company);
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