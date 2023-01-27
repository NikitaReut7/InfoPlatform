using AutoMapper;
using CommandService.Data;
using CommandService.DTOs;
using CommandService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers;

[Route("api/c/platforms/{platformId}/[controller]")]
[ApiController]
public class CommandsController : ControllerBase
{
    private readonly ICommandRepository _repository;
    private readonly IMapper _mapper;

    public CommandsController(ICommandRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<CommandReadDto>> GetCommandsForPlatform(int platformId)
    {
        Console.WriteLine($"--> Hit GetCommandsForPlatform:{platformId}");

        if (!_repository.PlatformExist(platformId))
        {
            return NotFound();
        }

        var commands = _repository.GetCommandsForPlatform(platformId);

        var commandsReadDtos = _mapper.Map<IEnumerable<CommandReadDto>>(commands);

        return Ok(commandsReadDtos);
    }

    [HttpGet("{commandId}", Name = "GetCommandForPlatform")]
    public ActionResult<CommandReadDto> GetCommandForPlatform(int platformId, int commandId)
    {
        Console.WriteLine($"--> Hit GetCommandForPlatform: {platformId} / {commandId}");

        if (!_repository.PlatformExist(platformId))
        {
            return NotFound();
        }

        var command = _repository.GetCommand(platformId,commandId);

        if(command == null)
        {
            return NotFound();
        }

        var commandReadDto = _mapper.Map<CommandReadDto>(command);

        return Ok(commandReadDto);
    }

    [HttpPost]
    public ActionResult<CommandReadDto> CreateCommandForPlatform(int platformId, CommandCreateDto commandDto)
    {
        Console.WriteLine($"--> Hit CreateCommandForPlatform: {platformId}");

        if (!_repository.PlatformExist(platformId))
        {
            return NotFound();
        }

        var command = _mapper.Map<Command>(commandDto);

        _repository.CreateCommand(platformId, command);
        _repository.SaveChanges();

        var commandReadDto = _mapper.Map<CommandReadDto>(command);

        return CreatedAtAction(nameof(CreateCommandForPlatform), new {platformId = platformId, commandId = commandReadDto.Id }, commandReadDto);
    }
}

