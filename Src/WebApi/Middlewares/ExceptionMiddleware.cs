using System.Net;
using Application.DTOs;
using Newtonsoft.Json;

namespace WebApi.Middlewares;

public class ExceptionMiddleware
{
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly RequestDelegate _next;
    public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger, RequestDelegate next)
    {
        _logger = logger;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            httpContext.Response.StatusCode = (int) HttpStatusCode.BadRequest;
            httpContext.Response.ContentType = "application/json";
            BaseMessageDto baseMessageDto = new BaseMessageDto()
            {
                Errors = ex
            };
            await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(baseMessageDto));
        }
    }
}