using System.Text;
using Application.Extensions;
using Infrastructure.Common.ConfigModels;
using Infrastructure.Common.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace WebApi.Extensions.Configuration
{
	public static class WebApiConfiguration
	{
        public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddInfrastructureConfiguration(configuration)
                .AddApplicationConfiguration(configuration)
                .AddJwtAuthentication(configuration);
            return services;
        }
        private static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            JwtConfig jwtConfig = new();
            configuration.Bind("Jwt", jwtConfig);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.TokenKey)),
                        ValidateIssuer = true,
                        ValidIssuer = jwtConfig.Issuer,
                        ValidateAudience = true,
                        ValidAudience = jwtConfig.Audience
                    };
                }); 
            return services;
        }
    }
}

