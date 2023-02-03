using Info.PlatformService.DTOs;

namespace Info.PlatformService.SyncDataServices.Http;
public interface ICommandDataClient
{
    Task SendPlatformToCommand(PlatformReadDto platform);
}

