using Info.CommandService.Models;

namespace Info.CommandService.SyncDataServices.Grps;

public interface IPlatformDataClient
{
    IEnumerable<Platform> ReturnAllPlatforms();
} 