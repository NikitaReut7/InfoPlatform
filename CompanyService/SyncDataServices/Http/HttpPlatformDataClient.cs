using AutoMapper;
using CompanyService.DTOs;
using CompanyService.SyncDataServices.Http;
using System.Text;
using System.Text.Json;

namespace CompanyService.SyncDataServices.Http
{
    public class HttpPlatformDataClient : IPlatformDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public HttpPlatformDataClient(HttpClient httpClient, IConfiguration configuration, IMapper mapper)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _mapper = mapper;
            
        }

        public async Task SendCompanyToPlatform(CompanyReadDto company)
        {
            var companyPublishedDto = _mapper.Map<CompanyPublishedDto>(company);

            var httpContent = new StringContent(
                JsonSerializer.Serialize(companyPublishedDto),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync(_configuration["PlatformService"], httpContent);

            if(response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Sync POST to PlatformService was OK!");
            }
            else
            {
                Console.WriteLine("--> Sync POST to PlatformService was NOT OK!");
            }
        }
    }
}