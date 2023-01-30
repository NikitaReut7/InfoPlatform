﻿using PlatformService.AsyncDataServices;
using PlatformService.EventProcessing;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace CommandService.AsyncDataServices;

public class MessageBusSubscriber : BackgroundService
{
    private readonly IGetConnection _getConnection;
    private readonly IEventProcessor _eventProcessor;
    private IConnection _connection;
    private IModel _channel;
    private readonly string _queueName;
    private readonly string _routingKey;
    private readonly IConfiguration _configuration;


    public MessageBusSubscriber(
        IEventProcessor eventProcessor,
        IGetConnection getConnection,
        IConfiguration configuration)
    {
        _getConnection = getConnection;
        _eventProcessor = eventProcessor;
        _configuration = configuration;
        _queueName = _configuration["RabbitMqConsumerQueue"];
        _routingKey = _configuration["RabbitMqCompaniesKey"];


        InitializeRabbitMQ();
    }

    private void InitializeRabbitMQ()
    {
        _connection = _getConnection.GetRBConnection();

        _channel = _connection.CreateModel();

        _channel.ExchangeDeclare(
            exchange: "trigger",
            type: ExchangeType.Direct);

        _channel.QueueDeclare(
            queue: _queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        _channel.QueueBind(
            queue: _queueName,
            exchange: "trigger",
            routingKey: _routingKey);

        Console.WriteLine("--> Listening on the Message Bus...");

        _connection.ConnectionShutdown += RabbitMQ_ConnectionShutDown;
    }

    private void RabbitMQ_ConnectionShutDown(object? sender, ShutdownEventArgs e)
    {
        Console.WriteLine("--> Connection shutdown");

    }

    public override void Dispose()
    {
        if (_channel.IsOpen)
        {
            _channel.Close();
            _connection.Close();
        }

        base.Dispose();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += (ModuleHandle, ea) =>
        {
            Console.WriteLine("--> Event Received!");

            var body = ea.Body;

            var notificationMessage = Encoding.UTF8.GetString(body.ToArray());

            _eventProcessor.ProcessEvent(notificationMessage);
        };

        _channel.BasicConsume(
            queue: _queueName,
            autoAck: true,
            consumer: consumer);

        return Task.CompletedTask;
    }
}
