using System;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions.Configuration
{
	public static class InfrastructureConfiguration
	{
		public static IServiceCollection AddInfrastructureConfiguration(this IServiceCollection services, IConfiguration configuration)
		{
			services.ConfigureEntityFramework(configuration);
			return services;
		}
		private static IServiceCollection ConfigureEntityFramework(this IServiceCollection services, IConfiguration configuration)
		{
            services.AddDbContext<WgDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnectionString"));
            });
			return services;
        }
	}
}

