using CommandService.Models;

namespace CommandService.SyncDataServices.Grps;

public interface IPlatformDataClient
{
    IEnumerable<Platform> ReturnAllPlatforms();
} 