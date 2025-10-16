using FluentValidation;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using ProjectTracker.Abstractions.ConfigurationObjects;
using ProjectTracker.Abstractions.Constants;
using ProjectTracker.Abstractions.Extensions;
using ProjectTracker.Api.ObjectStorage.Middlewares;
using ProjectTracker.Contracts.Events.Interfaces;
using ProjectTracker.Core.ObjectStorage;
using ProjectTracker.Core.ObjectStorage.Interfaces;
using ProjectTracker.Core.Services;
using ProjectTracker.Core.Services.Interfaces;
using ProjectTracker.Infrastructure;
using ProjectTracker.Infrastructure.Repositories;
using ProjectTracker.Infrastructure.Repositories.Interfaces;
using RabbitMQ.Client;
using Serilog;
using System.Reflection;
using System.Text.Json.Serialization;

namespace ProjectTracker.Api;

public static class DependencyInjection
{
	public static void AddMasstransitConfiguration(this IServiceCollection services, IConfiguration configuration)
	{
		var rabbitMqOptions = GetRabbitMqConfiguration(configuration);

		services.AddMassTransit
		(
			x => x.UsingRabbitMq(
				(context, configuration) =>
				{
					configuration.Host(rabbitMqOptions.Host, rabbitMqOptions.VirtualHost,
						x =>
						{
							x.Username(rabbitMqOptions.Username);
							x.Password(rabbitMqOptions.Password);
						});

					configuration.Message<IEventWrapper>(
						x =>
						{
							x.SetEntityName(rabbitMqOptions.DefaultEndpoint.Name);
						});

					configuration.Publish<IEventWrapper>(
						x =>
						{
							x.ExchangeType = ExchangeType.Topic;
						});

					configuration.ReceiveEndpoint(rabbitMqOptions.HistoryEndpoint.Name,
						x =>
						{
							x.Bind(rabbitMqOptions.DefaultEndpoint.Name, s =>
							{
								s.RoutingKey = rabbitMqOptions.HistoryEndpoint.RoutingKey;
								s.ExchangeType = ExchangeType.Topic;
							});
						}
					);

					configuration.ReceiveEndpoint(rabbitMqOptions.ReportInputEndpoint.Name,
						x =>
						{
							x.Bind(rabbitMqOptions.DefaultEndpoint.Name, s =>
							{
								s.RoutingKey = rabbitMqOptions.ReportInputEndpoint.RoutingKey;
								s.ExchangeType = ExchangeType.Topic;
							});
						}
					);

					configuration.ReceiveEndpoint(rabbitMqOptions.ReportResultEndpoint.Name,
						x =>
						{
							x.Bind(rabbitMqOptions.DefaultEndpoint.Name, s =>
							{
								s.RoutingKey = rabbitMqOptions.ReportResultEndpoint.RoutingKey;
								s.ExchangeType = ExchangeType.Topic;
							});
						}
					);
				})
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

	public static void AddValidationConfiguration(this IServiceCollection services)
	{
		//TODO проверить валидацию будет ли работать без Mediator pipeline может что-то еще нужно дописать на текущем моменте не возможно проверить
		ValidatorOptions.Global.DefaultClassLevelCascadeMode = CascadeMode.Continue;
		ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;
		services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
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
		services.AddScoped<IEmployeeService, EmployeeService>();
		services.AddScoped<ITaskService, TaskService>();
		services.AddScoped<IProjectService, ProjectService>();
		services.AddScoped<IReportService, ReportService>();
		services.AddScoped<IGroupService, GroupService>();
		services.AddScoped<IEventCollector, EventCollector>();
		services.AddScoped<IEventDispatcher, EventDispatcher>();
		services.AddSingleton<IReportEventAwaiter, ReportEventAwaiter>();
		services.AddScoped<IEventPublisher, EventPublisher>();
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

	public static void AddMiddlewares(this IServiceCollection services)
	{
		services.AddScoped<EventMiddleware>();
	}

	private static ProjectTrackerRabbitMqConfiguration GetRabbitMqConfiguration(IConfiguration configuration)
	{
		return configuration
			.GetRequiredSection(EnvironmentConstants.RabbitMqKey)
			.GetRequired<ProjectTrackerRabbitMqConfiguration>();
	}
}