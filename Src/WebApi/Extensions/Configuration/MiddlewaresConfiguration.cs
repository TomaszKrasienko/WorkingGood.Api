using WebApi.Middlewares;

namespace WebApi.Extensions.Configuration;

public static class MiddlewaresConfiguration
{
    public static WebApplication AddCustomMiddlewares(this WebApplication app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
        return app;
    }
}