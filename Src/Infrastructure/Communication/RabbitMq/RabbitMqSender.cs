using System.Text;
using System.Text.Json.Nodes;
using Domain.Enums;
using Domain.Interfaces.Communication;
using Domain.ValueObjects;
using Infrastructure.Common.ConfigModels;
using Newtonsoft.Json;
using NLog.Fluent;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using WorkingGood.Log;

namespace Infrastructure.Communication;

public class RabbitMqSender : IBrokerSender
{
    private readonly IWgLog<RabbitMqSender> _logger;
    private readonly RabbitMqConfig _rabbitMqConfig;
    public RabbitMqSender(IWgLog<RabbitMqSender> logger, RabbitMqConfig rabbitMqConfig)
    {
        _logger = logger;
        _rabbitMqConfig = rabbitMqConfig;
    }
    public void Send<T>(MessageDestinations messageDestinations, T obj)
    {
        //Todo: do poprawy
        try
        {
            ConnectionFactory connectionFactory = new ConnectionFactory();
            connectionFactory.HostName = _rabbitMqConfig.Host;
            connectionFactory.UserName = _rabbitMqConfig.UserName;
            connectionFactory.Password = _rabbitMqConfig.Password;
            if (_rabbitMqConfig.Port != null)
            {
                connectionFactory.Port = (int) _rabbitMqConfig.Port;
            }

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
        catch (BrokerUnreachableException exception)
        {
            _logger.Error(exception);
            throw;
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