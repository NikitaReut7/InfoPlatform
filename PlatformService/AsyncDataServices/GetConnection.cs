using RabbitMQ.Client;

namespace PlatformService.AsyncDataServices
{
    public class GetConnection : IGetConnection
    {
        private IConnection _connection;
        private readonly IConfiguration _configuration;

        public GetConnection(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IConnection GetRBConnection()
        {
            if (_connection == null)
            {
                var factory = new ConnectionFactory()
                {
                    HostName = _configuration["RabbitMqHost"],
                    Port = int.Parse(_configuration["RabbitMqPort"])
                };

                _connection = factory.CreateConnection();
            }

            return _connection;
                
        }
    }
}
