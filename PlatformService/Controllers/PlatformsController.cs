using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.DTOs;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace PlatformService.Controllers;

[Route("api/p/companies/{companyId}/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{
    private readonly IPlatformRepo _repository;
    private readonly IMapper _mapper;
    private readonly ICommandDataClient _commandDataClient;
    private readonly IMessageBusClient _messageBusClient;

    public PlatformsController(
        IPlatformRepo repository, 
        IMapper mapper,
        ICommandDataClient commandDataClient,
        IMessageBusClient messageBusClient)
    {
        _repository = repository;
        _mapper = mapper;
        _commandDataClient = commandDataClient;
        _messageBusClient = messageBusClient;
    }



    [HttpGet]
    public ActionResult<IEnumerable<PlatformReadDto>> GetPlatformsForCompany(int companyId)
    {
        Console.WriteLine($"--> Hit GetPlatformsForCompany:{companyId}");

        if (!_repository.CompanyExist(companyId))
        {
            return NotFound();
        }

        var platforms = _repository.GetPlatformsForCompany(companyId);

        var platformsReadDtos = _mapper.Map<IEnumerable<PlatformReadDto>>(platforms);

        return Ok(platformsReadDtos);
    }

    [HttpGet("{platformId}", Name = "GetPlatformForCompany")]
    public ActionResult<PlatformReadDto> GetPlatformForCompany(int companyId, int platformId)
    {
        Console.WriteLine($"--> Hit GetPlatformForCompany: {companyId} / {platformId}");

        if (!_repository.CompanyExist(companyId))
        {
            return NotFound();
        }

        var platform = _repository.GetPlatformForCompany(companyId, platformId);

        if (platform == null)
        {
            return NotFound();
        }

        var platformReadDto = _mapper.Map<PlatformReadDto>(platform);

        return Ok(platformReadDto);
    }

    [HttpPost]
    public async Task<ActionResult<PlatformReadDto>> CreatePlatformForCompany(int companyId, PlatformCreateDto platformCreateDto)
    {
        Console.WriteLine($"--> Hit CreatePlatformForCompany: {companyId}");

        if (!_repository.CompanyExist(companyId))
        {
            return NotFound();
        }

        var platform = _mapper.Map<Platform>(platformCreateDto);

        _repository.CreatePlatform(companyId, platform);
        _repository.SaveChanges();

        var platformReadDto = _mapper.Map<PlatformReadDto>(platform);

        // Send Sync Message
        try
        {
            await _commandDataClient.SendPlatformToCommand(platformReadDto);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"--> Could not send synchronously: {ex.Message}");
        }

        // Send Async Message
        try
        {
            var platformPublishedDto = _mapper.Map<PlatformPublishedDto>(platformReadDto);
            platformPublishedDto.Event = "Platform_Published";
            _messageBusClient.PublishNewPlatform(platformPublishedDto);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"--> Could not send asynchronously: {ex.Message}");
        }

        return CreatedAtAction(nameof(CreatePlatformForCompany), new { companyId = companyId, platformId = platformReadDto.Id }, platformReadDto);
    }
}

