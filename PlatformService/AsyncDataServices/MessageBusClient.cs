using PlatformService.DTOs;
using RabbitMQ.Client;
using RabbitMQ.Client.Logging;
using System.Text;
using System.Text.Json;

namespace PlatformService.AsyncDataServices;
public class MessageBusClient : IMessageBusClient
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly IGetConnection _getConnection;
    private readonly string _queueName;
    private readonly string _routingKey;
    private readonly IConfiguration _configuration;

    public MessageBusClient(IGetConnection getConnection, IConfiguration configuration)
    {
        _getConnection = getConnection;
        _configuration = configuration;

        try
        {
            _connection = _getConnection.GetRBConnection();
            _channel = _connection.CreateModel();
            _queueName = _configuration["RabbitMqProducerQueue"];
            _routingKey = _configuration["RabbitMqPlatformsKey"];


            _channel.QueueDeclare(queue: _queueName,
                   durable: true,
                   exclusive: false,
                   autoDelete: false,
                   arguments: null);

            _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Direct);

            _connection.ConnectionShutdown += RabbitMq_ConnectionShutDown;

            Console.WriteLine("--> Connected to MessageBus");

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not connect to the Message Bus {ex.Message}");
        }
    }

    private void RabbitMq_ConnectionShutDown(object? sender, ShutdownEventArgs e)
    {
        Console.WriteLine("--> RabbitMq connection shutdown");
    }

    public void PublishNewPlatform(PlatformPublishedDto platformPublishedDto)
    {
        var message = JsonSerializer.Serialize(platformPublishedDto);

        if (_connection.IsOpen)
        {
            Console.WriteLine("--> RabbitMq connection open, sending message...");
            SendMessage(message);

        }
        else
        {
            Console.WriteLine("--> RabbitMq connection closed, not sending");
        }
    }

    private void SendMessage(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);

        _channel.BasicPublish(
            exchange: "trigger",
            routingKey: _routingKey,
            basicProperties: null,
            body: body);

        Console.WriteLine($"--> We have sent {message}");

    }

    private void Dispose()
    {
        Console.WriteLine($"--> MessageBus Disposed");

        if (_channel.IsOpen)
        {
            _channel.Close();
            _connection.Close();
        }
    }
}

