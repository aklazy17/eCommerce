using RabbitMQ.Client;

namespace eCommerce.RabbitMq.Connection;

public class RabbitMqConnection : IRabbitMqConnection, IDisposable
{
    private readonly RabbitMqConfig _config;
    private IConnection? _connection;

    public IConnection Connection => _connection!;

    public RabbitMqConnection(RabbitMqConfig config)
    {
        _config = config;
        InitializeConnectionAsync().GetAwaiter();
    }

    private async Task InitializeConnectionAsync()
    {
        var factory = new ConnectionFactory()
        {
            HostName = _config.HostName,
            Port = _config.Port,
            UserName = _config.UserName,
            Password = _config.Password
        };

        _connection = await factory.CreateConnectionAsync();
    }

    public void Dispose()
    {
        _connection?.Dispose();
    }
}