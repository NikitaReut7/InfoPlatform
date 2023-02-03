using Info.CommandService.Models;

namespace Info.CommandService.Data;

public interface ICommandRepository
{
    bool SaveChanges();

    // Platforms
    IEnumerable<Platform> GetPlatforms();

    void CreatePlatform(Platform platform);
    void UpdatePlatform(Platform platform);
    void DeletePlatform(Platform platform);


    Platform GetPlatformByExternalId(int platformId);

    bool PlatformExistByExternalId(int externalPlatformId);

    // Commands

    IEnumerable<Command> GetCommandsForPlatform(int platformId);
    Command GetCommandForPlatform(int platformId, int commandId);

    Command GetCommandById(int id);

    void CreateCommand(int platformId, Command platform);
    void UpdateCommand(Command command);
    void DeleteCommand(Command command);
}

