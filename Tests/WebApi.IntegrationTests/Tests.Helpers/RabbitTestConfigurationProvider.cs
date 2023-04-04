using Infrastructure.Common.ConfigModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.IntegrationTests.Tests.Helpers;

public static class RabbitTestConfigurationProvider
{
    internal static IServiceCollection ConfigureTestRabbitMq(this IServiceCollection services)
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.Local.json")
            .AddEnvironmentVariables() 
            .Build();
        RabbitMqConfig rabbitMqConfig = new();
        config.Bind("Tests:RabbitMq", rabbitMqConfig);
        var originalService = services.SingleOrDefault(x => x.ServiceType == typeof(RabbitMqConfig));
        services.Remove(originalService);
        services.AddSingleton(rabbitMqConfig);
        return services;
    }
}