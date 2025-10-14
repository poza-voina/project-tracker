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

services.AddValidationConfiguration(); // TODO: ���������� ����� �� ��������

services.AddRepositories();

services.AddServices();

services.AddMiddlewares();

builder.AddSerilog();

var app = builder.Build();

app.UseExceptionHandler(_ => { });

app.UseMiddleware<EventMiddleware>();

app.MapControllers();

//TODO: Сделать как я обычно поступал т.е Сделать также как и MigrationRunner но Seeder
using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
await DatabaseSeeder.TrySeedAsync(context);


app.UseSwagger();

app.UseSwaggerUI();

app.Run();