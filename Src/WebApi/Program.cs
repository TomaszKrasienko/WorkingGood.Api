using WebApi.Common.Extensions.Configuration;
using WebApi.Common.Statics;
using WebApi.Extensions.Configuration;
using WorkingGood.Log;
using WorkingGood.Log.Configuration;


var builder = WebApplication.CreateBuilder(args);
var logger = WgLogger.CreateInstance(builder.Configuration);
try
{
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddConfiguration(builder.Configuration);
    builder.Services.UseWgLog(builder.Configuration, "WorkingGood.API");
    
    var app = builder.Build();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
    app.UseHttpsRedirection();

    app.UseCors(ConfigurationConst.CORS_POLICY_NAME);
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();
    app.AddCustomMiddlewares();
    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex);
}

public partial class Program {}