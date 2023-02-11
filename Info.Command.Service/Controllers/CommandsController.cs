using AutoMapper;
using Info.CommandService.Data.CommandRepository;
using Info.CommandService.Data.PlatformRepository;
using Info.CommandService.DTOs;
using Info.CommandService.Models;
using Microsoft.AspNetCore.Mvc;

namespace Info.CommandService.Controllers;

[Route("api/c/platforms/{platformId}/[controller]")]
[ApiController]
public class CommandsController : ControllerBase
{
    private readonly ICommandRepository _commandRepository;
    private readonly IPlatformRepository _platformRepository;
    private readonly IMapper _mapper;

    public CommandsController(
        ICommandRepository commandRepository,
        IPlatformRepository platformRepository,
        IMapper mapper)
    {
        _commandRepository = commandRepository;
        _platformRepository = platformRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<CommandReadDto>> GetCommandsForPlatform(int platformId)
    {
        Console.WriteLine($"--> Hit GetCommandsForPlatform:{platformId}");

        if (!_platformRepository.EntityExist(c => c.ExternalId == platformId))
        {
            return NotFound();
        }

        var commands = _commandRepository.GetAll(c=>c.PlatformId == platformId);

        var commandsReadDtos = _mapper.Map<IEnumerable<CommandReadDto>>(commands);

        return Ok(commandsReadDtos);
    }

    [HttpGet("{commandId}", Name = "GetCommandForPlatform")]
    public ActionResult<CommandReadDto> GetCommandForPlatform(int platformId, int commandId)
    {
        Console.WriteLine($"--> Hit GetCommandForPlatform: {platformId} / {commandId}");

        if (!_platformRepository.EntityExist(c=>c.ExternalId == platformId))
        {
            return NotFound();
        }

        var command = _commandRepository.Get(c=>c.PlatformId == platformId && c.Id == commandId);

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

        if (!_platformRepository.EntityExist(c=>c.ExternalId == platformId))
        {
            return NotFound();
        }

        var command = _mapper.Map<Command>(commandDto);
        command.PlatformId = platformId;

        _commandRepository.Create(command);
        _commandRepository.SaveChanges();

        var commandReadDto = _mapper.Map<CommandReadDto>(command);

        return CreatedAtAction(nameof(CreateCommandForPlatform), new { platformId = platformId, commandId = commandReadDto.Id }, commandReadDto);
    }

    [HttpPut("{commandId}", Name = "UpdateCommandForPlatform")]
    public ActionResult<CommandReadDto> UpdateCommandForPlatform(int platformId, int commandId, CommandUpdateDto commandUpdateDto)
    {
        Console.WriteLine($"--> Hit UpdateCommand: {commandId} for platform: {platformId}");

        var command = _commandRepository.Get(c => c.PlatformId == platformId && c.Id == commandId);
        if (command == null)
        {
            return NotFound();
        }

        command.HowTo = commandUpdateDto.HowTo;
        command.CommandLine = commandUpdateDto.CommandLine;

        _commandRepository.Update(command);
        _commandRepository.SaveChanges();

        var commandReadDto = _mapper.Map<CommandReadDto>(command);

        return Ok(commandReadDto);
    }

    [HttpDelete("{commandId}", Name = "DeleteCommandForPlatform")]
    public ActionResult<CommandReadDto> DeleteCommandForPlatform(int platformId, int commandId)
    {
        Console.WriteLine($"--> Hit DeleteCommand: {commandId} for platform: {platformId}");

        var command = _commandRepository.Get(c => c.PlatformId == platformId && c.Id == commandId);

        if (command == null)
        {
            return NotFound();
        }

        _commandRepository.Remove(command);
        _commandRepository.SaveChanges();

        return NoContent();
    }
}

