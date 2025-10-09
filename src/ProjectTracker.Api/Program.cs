using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using ProjectTracker.Abstractions.Constants;
using ProjectTracker.Api;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;


services.AddCorsConfiguration();

services.AddDbContextConfiguration(configuration);

services.AddExceptionHandler<ExceptionHandler>();

services.AddControllerConfiguration();

services.AddEndpointsApiExplorer();

services.AddSwaggerGenConfiguration(configuration);

services.AddValidationConfiguration(); // TODO: посмотреть будут ли работать

services.AddRepositories();

services.AddServices();

builder.AddSerilog();

var app = builder.Build();

app.UseExceptionHandler(_ => { });
app.MapControllers();

app.UseSwagger();

app.UseSwaggerUI();

app.Run();