using AutoMapper;
using CommandService.Data;
using CommandService.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers;

[Route("api/c/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{
    private readonly ICommandRepository _repository;
    private readonly IMapper _mapper;

    public PlatformsController(ICommandRepository repository, IMapper mapper)
	{
		_repository = repository;
		_mapper = mapper;
	}

	[HttpGet]
	public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
	{
		Console.WriteLine("--> Getting Platforms from Command Service");

		var platforms = _repository.GetPlatforms();

		var platformsReadDtos = _mapper.Map<IEnumerable<PlatformReadDto>>(platforms);

		return Ok(platformsReadDtos);
	}

	[HttpPost]
	public ActionResult TetsInboundConnection() 
	{
		Console.WriteLine("--> Inbound post # Command Service");

		return Ok("Inbound test of from Platforms Controller");
	}
}

