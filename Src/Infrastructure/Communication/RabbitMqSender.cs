using System.Text;
using System.Text.Json.Nodes;
using Domain.Enums;
using Domain.Interfaces.Communication;
using Infrastructure.Common.ConfigModels;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Infrastructure.Communication;

public class RabbitMqSender : IBrokerSender
{
    private readonly RabbitMqConfig _rabbitMqConfig;
    public RabbitMqSender(RabbitMqConfig rabbitMqConfig)
    {
        _rabbitMqConfig = rabbitMqConfig;
    }
    public void Send<T>(MessageDestinations messageDestinations, T obj)
    {
        ConnectionFactory connectionFactory = new ConnectionFactory()
        {
            HostName = _rabbitMqConfig.Host,
            Port = _rabbitMqConfig.Port,
            UserName = _rabbitMqConfig.UserName,
            Password = _rabbitMqConfig.Password
        };
        RabbitMqRoutesConfig routesConfig = GetRouteConfig(messageDestinations);
        using (var connection = connectionFactory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            var body = Encoding.UTF8.GetBytes(SerializeMessage<T>(obj));
            IBasicProperties properties = channel.CreateBasicProperties();
            properties.ContentType = "application/json";
            channel.BasicPublish(
                exchange: routesConfig.Exchange,
                routingKey: routesConfig.RoutingKey,
                basicProperties: properties,
                body: body
                );
        }
    }
    private string SerializeMessage<T>(T obj)
    {
        return JsonConvert.SerializeObject(obj);
    }
    private RabbitMqRoutesConfig GetRouteConfig(MessageDestinations messageDestinations)
    {
        return _rabbitMqConfig
            .SendingRoutes
            .FirstOrDefault(x => x.Destination == messageDestinations.ToString())!;
    }
}