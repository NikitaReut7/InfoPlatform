using RabbitMQ.Client;

namespace PlatformService.AsyncDataServices
{
    public interface IGetConnection
    {
        IConnection GetRBConnection();
    }
}
