using System;
using Domain.Interfaces;
using Domain.Interfaces.Communication;
using Domain.Interfaces.Validation;
using Infrastructure.Common.ConfigModels;
using Infrastructure.Communication;
using Infrastructure.Persistance;
using Infrastructure.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Common.Extensions.Configuration
{
	public static class InfrastructureConfiguration
	{
		public static IServiceCollection AddInfrastructureConfiguration(this IServiceCollection services, IConfiguration configuration)
		{
			services
				.ConfigureEntityFramework(configuration)
				.ConfigureConfigs(configuration)
				.ConfigureServices();
			return services;
		}

		private static IServiceCollection ConfigureServices(this IServiceCollection services)
		{
			services
				.AddScoped<IEmployeeChecker, EmployeeChecker>()
				.AddScoped<ICompanyChecker, CompanyChecker>()
				.AddScoped<IOfferChecker, OfferChecker>()
				.AddScoped<IBrokerSender, RabbitMqSender>()
			    .AddScoped<IUnitOfWork, UnitOfWork>();
			return services;
		}
		private static IServiceCollection ConfigureEntityFramework(this IServiceCollection services, IConfiguration configuration)
		{
            services.AddDbContext<WgDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnectionString")!);
            });
			return services;
        }
		private static IServiceCollection ConfigureConfigs(this IServiceCollection services,
			IConfiguration configuration)
		{
			RabbitMqConfig rabbitMqConfig = new();
			configuration.Bind("RabbitMq", rabbitMqConfig);
			services.AddSingleton(rabbitMqConfig);
			JwtConfig jwtConfig = new();
			configuration.Bind("Jwt", jwtConfig);
			services.AddSingleton(jwtConfig);
			return services;
		}
	}
}

