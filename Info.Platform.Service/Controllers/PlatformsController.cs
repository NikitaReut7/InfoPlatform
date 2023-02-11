using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Info.PlatformService.DTOs;
using Info.PlatformService.Models;
using Info.PlatformService.SyncDataServices.Http;
using Info.PlatformContracts;
using Info.PlatformService.Data.PlatformRepository;
using Info.PlatformService.Data.CompanyRepository;

namespace Info.PlatformService.Controllers;

[Route("api/p/companies/{companyId}/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{
    private readonly IPlatformRepository _platformRepository;
    private readonly ICompanyRepository _companyRepository;
    private readonly IMapper _mapper;
    private readonly ICommandDataClient _commandDataClient;
    private readonly IPublishEndpoint _publishEndpoint;

    public PlatformsController(
        IPlatformRepository platformRepository,
        ICompanyRepository companyRepository,
        IMapper mapper,
        ICommandDataClient commandDataClient,
        IPublishEndpoint publishEndpoint)
    {
        _platformRepository = platformRepository;
        _companyRepository = companyRepository;
        _mapper = mapper;
        _commandDataClient = commandDataClient;
        _publishEndpoint = publishEndpoint;
    }



    [HttpGet]
    public ActionResult<IEnumerable<PlatformReadDto>> GetPlatformsForCompany(int companyId)
    {
        Console.WriteLine($"--> Hit GetPlatformsForCompany:{companyId}");

        if (!_companyRepository.EntityExist(companyId))
        {
            return NotFound();
        }

        var platforms = _platformRepository.GetAll(c => c.CompanyId == companyId);

        var platformsReadDtos = _mapper.Map<IEnumerable<PlatformReadDto>>(platforms);

        return Ok(platformsReadDtos);
    }

    [HttpGet("{platformId}", Name = "GetPlatformForCompany")]
    public ActionResult<PlatformReadDto> GetPlatformForCompany(int companyId, int platformId)
    {
        Console.WriteLine($"--> Hit GetPlatformForCompany: {companyId} / {platformId}");

        if (!_companyRepository.EntityExist(companyId))
        {
            return NotFound();
        }

        var platform = _platformRepository.Get(c => c.CompanyId == companyId && c.Id == platformId);

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

        if (!_companyRepository.EntityExist(companyId))
        {
            return NotFound();
        }

        var platform = _mapper.Map<Platform>(platformCreateDto);
        platform.CompanyId = companyId;

        _platformRepository.Create(platform);
        _platformRepository.SaveChanges();

        var platformReadDto = _mapper.Map<PlatformReadDto>(platform);

        // Send Sync Message
        // try
        // {
        //     await _commandDataClient.SendPlatformToCommand(platformReadDto);
        // }
        // catch (Exception ex)
        // {
        //     Console.WriteLine($"--> Could not send synchronously: {ex.Message}");
        // }

        // Send Async Message
        try
        {
            await _publishEndpoint.Publish(new PlatformCreated(platformReadDto.Id, platformReadDto.Name));

        }
        catch (Exception ex)
        {
            Console.WriteLine($"--> Could not send asynchronously: {ex.Message}");
        }

        return CreatedAtAction(nameof(CreatePlatformForCompany), new { companyId = companyId, platformId = platformReadDto.Id }, platformReadDto);
    }

    [HttpPut("{platformId}", Name = "UpdatePlatformForCompany")]
    public async Task<ActionResult<PlatformReadDto>> UpdatePlatformForCompany(int companyId, int platformId, PlatformUpdateDto platformUpdateDto)
    {
        Console.WriteLine($"--> Hit UpdatePlatform: {platformId} for company: {companyId}");

        var platform = _platformRepository.Get(c => c.CompanyId == companyId && c.Id == platformId);

        if (platform == null)
        {
            return NotFound();
        }

        platform.Name = platformUpdateDto.Name;
        platform.Cost = platformUpdateDto.Cost;

        _platformRepository.Update(platform);
        _platformRepository.SaveChanges();

        var platformReadDto = _mapper.Map<PlatformReadDto>(platform);


        // Send Async Message
        try
        {
            await _publishEndpoint.Publish(new PlatformUpdated(platformReadDto.Id, platformReadDto.Name));

        }
        catch (Exception ex)
        {
            Console.WriteLine($"--> Could not send asynchronously: {ex.Message}");
        }

        return Ok(platformReadDto);
    }

    [HttpDelete("{platformId}", Name = "DeletePlatformForCompany")]
    public async Task<ActionResult> DeletePlatformForCompany(int companyId, int platformId)
    {
        Console.WriteLine($"--> Hit DeletePlatform: {platformId} for company: {companyId}");

        var platform = _platformRepository.Get(c => c.CompanyId == companyId && c.Id == platformId);

        if (platform == null)
        {
            return NotFound();
        }

        var platformReadDto = _mapper.Map<PlatformReadDto>(platform);

        _platformRepository.Remove(platform);
        _platformRepository.SaveChanges();

        // Send Async Message
        try
        {
            await _publishEndpoint.Publish(new PlatformDeleted(platformReadDto.Id));

        }
        catch (Exception ex)
        {
            Console.WriteLine($"--> Could not send asynchronously: {ex.Message}");
        }

        return NoContent();
    }
}

