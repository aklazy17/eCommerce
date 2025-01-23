using RabbitMQ.Client;

namespace eCommerce.RabbitMq.Connection;

public interface IRabbitMqConnection
{
    IConnection Connection { get; }
}