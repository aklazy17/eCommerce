using AutoMapper;
using eCommerce.Notification.API.Models.Dtos;
using eCommerce.Notification.API.Repositories;
using eCommerce.RabbitMq.Connection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace eCommerce.Notification.API.BackgroundServices;

public class NotificationConsumerHostedService : BackgroundService
{
    private const string RABBIT_MQ_QUEUE = "eComm-Notification";

    private readonly IRabbitMqConnection _connection;
    private readonly ILogger<NotificationConsumerHostedService> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IMapper _mapper;
    private IChannel? _channel;

    public NotificationConsumerHostedService(IRabbitMqConnection connection,
        ILogger<NotificationConsumerHostedService> logger,
        IServiceScopeFactory serviceScopeFactory,
        IMapper mapper)
    {
        _connection = connection;
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
        _mapper = mapper;

        InitRabbitMqAsync().GetAwaiter();
    }

    private async Task InitRabbitMqAsync()
    {
        if (_connection.Connection is null)
        {
            _logger.LogWarning("RabbitMQ connection is not established.");
            return;
        }

        _channel = await _connection.Connection.CreateChannelAsync();

        await _channel.QueueDeclareAsync(RABBIT_MQ_QUEUE, exclusive: false);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        if (_channel is not null)
        {
            var consumer = new AsyncEventingBasicConsumer(_channel!);
            consumer.ReceivedAsync += OnMessageQueueReceived;

            await _channel.BasicConsumeAsync(queue: RABBIT_MQ_QUEUE, autoAck: true, consumer: consumer, cancellationToken: stoppingToken);
        }
    }

    private async Task OnMessageQueueReceived(object sender, BasicDeliverEventArgs @event)
    {
        try
        {
            var body = @event.Body.ToArray();
            var message = Encoding.UTF8.GetString(body) ?? string.Empty;

            _logger.LogInformation($"RabbitMq Message Recieved: {message}");

            var result = JsonSerializer.Deserialize<NotificationDto>(message);

            var scope = _serviceScopeFactory.CreateAsyncScope();
            var notificationRepository = scope.ServiceProvider.GetRequiredService<INotificationRepository>();
            
            await notificationRepository.AddAsync(_mapper.Map<Models.Notification>(result));
        }
        catch (Exception ex)
        {
            _logger.LogError($"{ex}");
        }
    }

    public override void Dispose()
    {
        _channel?.Dispose();
        _connection?.Connection?.Dispose();
        base.Dispose();
    }
}