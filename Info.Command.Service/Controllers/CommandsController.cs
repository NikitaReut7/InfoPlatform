using AutoMapper;
using Info.CommandService.Data;
using Info.CommandService.DTOs;
using Info.CommandService.Models;
using Microsoft.AspNetCore.Mvc;

namespace Info.CommandService.Controllers;

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

        if (!_repository.PlatformExistByExternalId(platformId))
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

        if (!_repository.PlatformExistByExternalId(platformId))
        {
            return NotFound();
        }

        var command = _repository.GetCommandForPlatform(platformId, commandId);

        if (command == null)
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

        if (!_repository.PlatformExistByExternalId(platformId))
        {
            return NotFound();
        }

        var command = _mapper.Map<Command>(commandDto);

        _repository.CreateCommand(platformId, command);
        _repository.SaveChanges();

        var commandReadDto = _mapper.Map<CommandReadDto>(command);

        return CreatedAtAction(nameof(CreateCommandForPlatform), new { platformId = platformId, commandId = commandReadDto.Id }, commandReadDto);
    }

    [HttpPut("{commandId}", Name = "UpdateCommandForPlatform")]
    public ActionResult<CommandReadDto> UpdateCommandForPlatform(int platformId, int commandId, CommandUpdateDto commandUpdateDto)
    {
        Console.WriteLine($"--> Hit UpdateCommand: {commandId} for platform: {platformId}");

        var command = _repository.GetCommandForPlatform(platformId, commandId);
        if (command == null)
        {
            return NotFound();
        }

        command.HowTo = commandUpdateDto.HowTo;
        command.CommandLine = commandUpdateDto.CommandLine;

        _repository.UpdateCommand(command);
        _repository.SaveChanges();

        var commandReadDto = _mapper.Map<CommandReadDto>(command);

        return Ok(commandReadDto);
    }

    [HttpDelete("{commandId}", Name = "DeleteCommandForPlatform")]
    public ActionResult<CommandReadDto> DeleteCommandForPlatform(int platformId, int commandId)
    {
        Console.WriteLine($"--> Hit DeleteCommand: {commandId} for platform: {platformId}");

        var command = _repository.GetCommandForPlatform(platformId, commandId);

        if (command == null)
        {
            return NotFound();
        }

        _repository.DeleteCommand(command);
        _repository.SaveChanges();

        return NoContent();
    }
}

