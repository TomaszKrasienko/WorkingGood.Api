using NLog;
using NLog.Web;
using WebApi.Common.Extensions.Configuration;
using WebApi.Common.Statics;
using WebApi.Extensions.Configuration;

Logger logger = LogManager.GetLogger("RmqTarget");
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddConfiguration(builder.Configuration);
builder.Host.UseNLog();
var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});
app.AddCustomMiddlewares();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors(ConfigurationConst.CORS_POLICY_NAME);
app.MapControllers();
app.Run();

public partial class Program {}