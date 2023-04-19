using System.Text;
using Application.Common.Extensions.Configuration;
using Infrastructure.Common.ConfigModels;
using Infrastructure.Common.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using WebApi.Extensions.Configuration;

namespace WebApi.Common.Extensions.Configuration
{
	public static class WebApiConfiguration
	{
        public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddInfrastructureConfiguration(configuration)
                .AddApplicationConfiguration(configuration)
                .AddJwtAuthentication(configuration)
                .ConfigureSwagger()
                .AddCorsPolicy();
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
        private static IServiceCollection AddCorsPolicy(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(Common.Statics.ConfigurationConst.CORS_POLICY_NAME, builder =>
                {
                    builder
                        .SetIsOriginAllowed(_ => true)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });
            return services;
        }
    }
}

