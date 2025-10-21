using ProjectTracker.History.Api;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddDbContextConfiguration(configuration);

services.AddRepositories();

services.AddServices();

services.AddMasstransitConfiguration(configuration);

services.AddControllerConfiguration();

services.AddSwaggerGenConfiguration(configuration);

services.AddEndpointsApiExplorer();

builder.AddSerilog();

var app = builder.Build();

app.MapControllers();

app.UseSwagger();

app.UseSwaggerUI();

app.Run();