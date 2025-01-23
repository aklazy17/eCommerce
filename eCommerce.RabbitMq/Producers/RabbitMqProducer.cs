using eCommerce.RabbitMq.Connection;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace eCommerce.RabbitMq.Producers;

public class RabbitMqProducer : IMessageProducer
{
    private readonly IRabbitMqConnection _connection;

    public RabbitMqProducer(IRabbitMqConnection connection)
    {
        _connection = connection;
    }

    public async Task SendMessageAsync<T>(T message, string routingKey)
    {
        if (_connection.Connection is null)
        {
            throw new Exception("Connection to the RabbitMQ server could not be established. Please consult with the administrator.");
        }

        using var channel = await _connection.Connection.CreateChannelAsync();

        await channel.QueueDeclareAsync("", exclusive: false);

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        await channel.BasicPublishAsync(exchange: "", routingKey: routingKey, body: body);
    }
}