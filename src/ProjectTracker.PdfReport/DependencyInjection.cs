using MassTransit;
using ProjectTracker.Abstractions.ConfigurationObjects;
using ProjectTracker.Abstractions.Constants;
using ProjectTracker.Abstractions.Extensions;
using ProjectTracker.Contracts.Events.Interfaces;
using ProjectTracker.Contracts.Events.PublishEvents.Shared;
using ProjectTracker.Contracts.Events.Reports;
using ProjectTracker.PdfReport.Data;
using ProjectTracker.PdfReport.Services;
using ProjectTracker.PdfReport.Services.Interfaces;
using RabbitMQ.Client;

namespace ProjectTracker.PdfReport;

public static class DependencyInjection
{
	public static void AddMasstransitConfiguration(this IServiceCollection services, IConfiguration configuration)
	{
		var rabbitMqOptions = GetRabbitMqConfiguration(configuration);

		services.AddMassTransit
		(
			x =>
			{
				x.AddConsumer<ReportInputTaskEventConsumer>();

				x.UsingRabbitMq(
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

					configuration.ReceiveEndpoint(rabbitMqOptions.ReportInputTaskEventEndpoint.Name,
						x =>
						{
							x.Bind(rabbitMqOptions.ReportInputEndpoint.Name, s =>
							{
								s.RoutingKey = rabbitMqOptions.ReportInputEndpoint.RoutingKey;
								s.ExchangeType = ExchangeType.Topic;
							});

							x.ConfigureConsumer<ReportInputTaskEventConsumer>(context);
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
				});
			}
		);
	}

	public static void AddServices(this IServiceCollection services)
	{
		services.AddScoped<IGeneratePdfService, GeneratePdfService>();
		services.AddScoped<IMinioService, MinioService>();
	}

	private static PdfReportRabbitMqConfiguration GetRabbitMqConfiguration(IConfiguration configuration)
	{
		return configuration
			.GetRequiredSection(EnvironmentConstants.RabbitMqKey)
			.GetRequired<PdfReportRabbitMqConfiguration>();
	}
}
