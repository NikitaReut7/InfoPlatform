using AutoMapper;
using CompanyService.DTOs;
using CompanyService.Models;

namespace CompanyService.Profiles;
public class CompaniesProfile : Profile
{
    public CompaniesProfile()
    {
        // Source -> Target
        CreateMap<Company, CompanyReadDto>();
        CreateMap<CompanyCreateDto, Company>();
        CreateMap<CompanyReadDto, CompanyPublishedDto>();
        CreateMap<CompanyReadDto, CompanyDeletedDto>();


    }
}
