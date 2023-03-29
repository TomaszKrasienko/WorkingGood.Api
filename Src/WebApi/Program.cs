﻿using NLog;
using NLog.Web;
using WebApi.Extensions.Configuration;

Logger logger = LogManager.GetLogger("RmqTarget");
logger.Info("Test");
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddConfiguration(builder.Configuration);
builder.Services.ConfigureSwagger();
builder.Host.UseNLog();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program {}