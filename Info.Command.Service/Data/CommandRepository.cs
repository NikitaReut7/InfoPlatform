using Info.CommandService.Models;
using System;

namespace Info.CommandService.Data;

public class CommandRepository : ICommandRepository
{
    private readonly AppDbContext _dbContext;

    public CommandRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void CreateCommand(int platformId, Command command)
    {
        if (command == null)
        {
            throw new ArgumentNullException(nameof(command));
        }

        command.PlatformId = platformId;
        _dbContext.Commands.Add(command);
    }

    public void CreatePlatform(Platform platform)
    {
        if (platform == null)
        {
            throw new ArgumentNullException(nameof(platform));
        }

        _dbContext.Platforms.Add(platform);
    }

    public void UpdateCommand(Command command)
    {
        if (command == null)
        {
            throw new ArgumentNullException(nameof(command));
        }

        _dbContext.Commands.Update(command);
    }

    public void UpdatePlatform(Platform platform)
    {
        if (platform == null)
        {
            throw new ArgumentNullException(nameof(platform));
        }

        _dbContext.Platforms.Update(platform);
    }

    public void DeleteCommand(Command command)
    {
        if (command == null)
        {
            throw new ArgumentNullException(nameof(command));
        }

        _dbContext.Commands.Remove(command);
    }

    public void DeletePlatform(Platform platform)
    {
        if (platform == null)
        {
            throw new ArgumentNullException(nameof(platform));
        }

        _dbContext.Platforms.Remove(platform);
    }


    public bool PlatformExistByExternalId(int externalPlatformId)
    {
        return _dbContext.Platforms.Any(c => c.ExternalId == externalPlatformId);

    }

    public Command GetCommandForPlatform(int platformId, int commandId)
    {
        return _dbContext.Commands
            .Where(c => c.PlatformId == platformId &&
                        c.Id == commandId)
            .FirstOrDefault();
    }

    public IEnumerable<Command> GetCommandsForPlatform(int platformId)
    {
        return _dbContext.Commands
            .Where(c => c.PlatformId == platformId)
            .OrderBy(c => c.Platform.Name);
    }

    public Command GetCommandById(int id)
    {
        return _dbContext.Commands
            .Where(c => c.Id == id)
            .FirstOrDefault();
    }

    public IEnumerable<Platform> GetPlatforms()
    {
        return _dbContext.Platforms.ToList();
    }

    public Platform GetPlatformByExternalId(int platformId)
    {
        return _dbContext.Platforms.FirstOrDefault(c => c.ExternalId == platformId);
    }

    public bool SaveChanges()
    {
        return (_dbContext.SaveChanges() >= 0);
    }
}

