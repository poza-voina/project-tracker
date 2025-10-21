using ProjectTracker.Api;
using ProjectTracker.Api.ObjectStorage.Middlewares;
using ProjectTracker.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddSigletonConfigurations(configuration);

services.AddCorsConfiguration();

services.AddMasstransitConfiguration(configuration);

services.AddDbContextConfiguration(configuration);

services.AddExceptionHandler<ExceptionHandler>();

services.AddControllerConfiguration();

services.AddEndpointsApiExplorer();

services.AddSwaggerGenConfiguration(configuration);

services.AddValidationConfiguration();

services.AddRepositories();

services.AddServices();

services.AddMiddlewares();

builder.AddSerilog();

var app = builder.Build();

app.UseExceptionHandler(_ => { });

app.UseMiddleware<EventMiddleware>();

app.MapControllers();

app.UseSwagger();

app.UseSwaggerUI();

app.Run();