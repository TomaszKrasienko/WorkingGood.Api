using System.Reflection;
using Application.CQRS.Companies.Commands;
using Application.CQRS.EmployeesAuth.Commands.Login;
using Application.CQRS.EmployeesAuth.Commands.Refresh;
using Application.CQRS.EmployeesAuth.Commands.VerifyEmployee;
using Application.EmployeesAuth.Commands;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions
{
    public static class ApplicationConfiguration
	{
		public static IServiceCollection AddApplicationConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services
	            .ConfigureValidators()
	            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
            return services;
		}

		private static IServiceCollection ConfigureValidators(this IServiceCollection services)
		{
			services.AddValidatorsFromAssemblyContaining<AddCompanyCommand>();			
			services.AddValidatorsFromAssemblyContaining<RegisterEmployeeCommand>();
			services.AddValidatorsFromAssemblyContaining<VerifyEmployeeCommand>();
			services.AddValidatorsFromAssemblyContaining<LoginCommand>();
			services.AddValidatorsFromAssemblyContaining<RefreshCommand>();
			return services;
		}
	}
}

