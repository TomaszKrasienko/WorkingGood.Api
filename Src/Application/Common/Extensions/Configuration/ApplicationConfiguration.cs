using System.Reflection;
using Application.CQRS.Companies.Commands.AddCompany;
using Application.CQRS.EmployeesAuth.Commands.Login;
using Application.CQRS.EmployeesAuth.Commands.Refresh;
using Application.CQRS.EmployeesAuth.Commands.VerifyEmployee;
using Application.EmployeesAuth.Commands;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Common.Extensions.Configuration
{
    public static class ApplicationConfiguration
	{
		public static IServiceCollection AddApplicationConfiguration(this IServiceCollection services, IConfiguration configuration)
		{
			services
				.ConfigureValidators()
				.ConfigureMediatr()
				.SetAutoMapper();
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
		private static IServiceCollection ConfigureMediatr(this IServiceCollection services)
		{
			return services
				.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
		}

		private static IServiceCollection SetAutoMapper(this IServiceCollection services)
		{
			return services
				.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
		}
	}
}

