using CommandService.Models;
using System;

namespace CommandService.Data;
public class CommandRepository : ICommandRepository
{
    private readonly AppDbContext _dbContext;

    public CommandRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void CreateCommand(int platformId, Command command)
    {
        if(command == null)
        {
            throw new ArgumentNullException(nameof(command));
        }

        command.PlatformId= platformId;
        _dbContext.Commands.Add(command);
    }

    public void CreatePlatform(Platform platform)
    {
        if(platform == null)
        {
            throw new ArgumentNullException(nameof(platform));
        }

        _dbContext.Platforms.Add(platform); 
    }

    public bool ExternalPlatformExist(int externalPlatformId)
    {
        return _dbContext.Platforms.Any(c => c.ExternalId == externalPlatformId);

    }

    public Command GetCommand(int platformId, int commandId)
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

    public IEnumerable<Platform> GetPlatforms()
    {
        return _dbContext.Platforms.ToList();
    }

    public bool PlatformExist(int platformId)
    {
        return _dbContext.Platforms.Any(c => c.Id == platformId);
    }

    public bool SaveChanges()
    {
        return (_dbContext.SaveChanges() >= 0);
    }
}

