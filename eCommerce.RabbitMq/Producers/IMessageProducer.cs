namespace eCommerce.RabbitMq.Producers;

public interface IMessageProducer
{
    Task SendMessageAsync<T>(T message, string routingKey);
}