using AutoMapper;
using Info.CommandService.Data;
using Info.CommandService.Data.PlatformRepository;
using Info.CommandService.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Info.CommandService.Controllers;

[Route("api/c/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{
    private readonly IPlatformRepository _repository;
    private readonly IMapper _mapper;

    public PlatformsController(IPlatformRepository repository, IMapper mapper)
	{
		_repository = repository;
		_mapper = mapper;
	}

	[HttpGet]
	public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
	{
		Console.WriteLine("--> Getting Platforms from Command Service");

		var platforms = _repository.GetAll();

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

