using System;
using Infrastructure.Extensions.Configuration;

namespace WebApi.Extensions.Configuration
{
	public static class WebApiConfiguration
	{
        public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddInfrastructureConfiguration(configuration);
            return services;
        }
    }
}

