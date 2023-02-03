using AutoMapper;
using Info.CompanyService.DTOs;
using Info.CompanyService.Models;

namespace Info.CompanyService.Profiles;
public class CompaniesProfile : Profile
{
    public CompaniesProfile()
    {
        // Source -> Target
        CreateMap<Company, CompanyReadDto>();
        CreateMap<Company, CompanyUpdateDto>();
        CreateMap<CompanyCreateDto, Company>();
        CreateMap<CompanyReadDto, CompanyPublishedDto>();
        CreateMap<CompanyReadDto, CompanyDeletedDto>();


    }
}
