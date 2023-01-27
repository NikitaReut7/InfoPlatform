using PlatformService.DTOs;
using RabbitMQ.Client;
using RabbitMQ.Client.Logging;
using System.Text;
using System.Text.Json;

namespace PlatformService.AsyncDataServices;
public class MessageBusClient : IMessageBusClient
{
    private readonly IConfiguration _configuration;
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public MessageBusClient(IConfiguration configuration)
    {
        _configuration = configuration;
        var factory = new ConnectionFactory()
        {
            HostName = _configuration["RabbitMqHost"],
            Port = int.Parse(_configuration["RabbitMqPort"])
        };

        try
        {
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);

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

        if(_connection.IsOpen)
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
            routingKey: "",
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

