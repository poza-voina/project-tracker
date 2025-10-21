using MassTransit;
using Microsoft.EntityFrameworkCore;
using ProjectTracker.Abstractions.Extensions;
using ProjectTracker.History.Api.ObjectStorate.Configuration;
using ProjectTracker.History.Api.ObjectStorate.Consumers;
using ProjectTracker.History.Core.Services;
using ProjectTracker.History.Core.Services.Interfaces;
using ProjectTracker.History.Infrastructure;
using ProjectTracker.History.Infrastructure.Repositories;
using ProjectTracker.History.Infrastructure.Repositories.Interfaces;
using Serilog;
using System.Reflection;
using System.Text.Json.Serialization;

namespace ProjectTracker.History.Api;

public static class DependencyInjection
{
	public static void AddMasstransitConfiguration(this IServiceCollection services, IConfiguration configuration)
	{
		var rabbitMqOptions = GetRabbitMqConfiguration(configuration);

		services.AddMassTransit
		(
			x =>
			{
				x.AddConsumer<HistoryConsumer>();

				x.UsingRabbitMq(
				(context, configuration) =>
				{
					configuration.Host(rabbitMqOptions.Host, rabbitMqOptions.VirtualHost,
						x =>
						{
							x.Username(rabbitMqOptions.Username);
							x.Password(rabbitMqOptions.Password);
						});

					configuration
						.ReceiveEndpoint(
							rabbitMqOptions.HistoryEndpoint.Name,
							x => x.ConfigureConsumer<HistoryConsumer>(context)
						);
				});
			}
		);
	}

	public static void AddSigletonConfigurations(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddSingleton(x => GetRabbitMqConfiguration(configuration));
	}

	public static void AddDbContextConfiguration(this IServiceCollection services, IConfiguration configuration)
	{
		var connectionSection = configuration.GetRequiredSection(EnvironmentConstants.ConnectionSection);
		var connectionString = connectionSection.GetRequiredValue<string>(EnvironmentConstants.DefaultConnectionStringKey);

		services.AddDbContext<ApplicationDbContext>(
			options => options
				.UseNpgsql(connectionString)
				.EnableSensitiveDataLogging()
				.LogTo(message => Log.Information(message),
				new[] { DbLoggerCategory.Database.Command.Name }));
	}

	public static void AddCorsConfiguration(this IServiceCollection services)
	{
		services.AddCors(options =>
		{
			options.AddDefaultPolicy(policy =>
			{
				policy
					.AllowAnyOrigin()
					.AllowAnyMethod()
					.AllowAnyHeader();
			});
		});
	}

	public static void AddControllerConfiguration(this IServiceCollection services)
	{
		services.AddControllers()
			.AddJsonOptions(options =>
			{
				options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
				options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
				options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
				options.JsonSerializerOptions.UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow;
			});

		services.ConfigureHttpJsonOptions(options =>
		{
			options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
		});
	}

	public static void AddSwaggerGenConfiguration(this IServiceCollection services, IConfiguration configuration) //TODO возможно нужно добавить версию api
	{
		services.AddSwaggerGen(x =>
		{
			var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
			var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

			x.IncludeXmlComments(xmlPath);

			//TODO: сделать/ посмотреть добавить фильтр для параметров а то они с большой буквы
			//x.OperationFilterDescriptors.Add(new FilterDescriptor
			//{
			//Type = typeof(RemoveQueryParameters<GetStatementQuery>),
			//Arguments = [new[] { nameof(GetStatementQuery.AccountId) }]
			//});

			//x.OperationFilter<CamelCaseQueryParametersFilter>();
		});
	}

	public static void AddServices(this IServiceCollection services)
	{
		services.AddScoped<ITaskHistoryService, TaskHistoryService>();
	}

	public static void AddRepositories(this IServiceCollection services)
	{
		services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
	}

	public static void AddSerilog(this WebApplicationBuilder builder)
	{

		builder.Host.UseSerilog((context, configuration) =>
			configuration.ReadFrom.Configuration(context.Configuration));
	}

	private static HistoryRabbitMqConfiguration GetRabbitMqConfiguration(IConfiguration configuration)
	{
		return configuration
			.GetRequiredSection(EnvironmentConstants.RabbitMqKey)
			.GetRequired<HistoryRabbitMqConfiguration>();
	}
}