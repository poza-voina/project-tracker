using MassTransit;
using Minio;
using ProjectTracker.Abstractions.Extensions;
using ProjectTracker.PdfReport.ObjectStorage.Consumers;
using ProjectTracker.PdfReport.ObjectStorage.Data;
using ProjectTracker.PdfReport.ObjectStorage.Services;
using ProjectTracker.PdfReport.ObjectStorage.Services.Interfaces;

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
				x.AddConsumer<ReportInputTaskGroupEventConsumer>();

				x.UsingRabbitMq(
				(context, configuration) =>
				{
					configuration.Host(rabbitMqOptions.Host, rabbitMqOptions.VirtualHost,
						x =>
						{
							x.Username(rabbitMqOptions.Username);
							x.Password(rabbitMqOptions.Password);
						});

					configuration.ReceiveEndpoint(rabbitMqOptions.ReportInputTaskEndpoint.Name,
						x =>
						{
							x.ConfigureConsumer<ReportInputTaskEventConsumer>(context);
						}
					);

					configuration.ReceiveEndpoint(rabbitMqOptions.ReportInputTaskGroupEndpoint.Name,
						x =>
						{
							x.ConfigureConsumer<ReportInputTaskGroupEventConsumer>(context);
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
		services.AddScoped<IProjectTrackerClient, ProjectTrackerClient>();
	}

	public static void AddMinioConfiguration(this IServiceCollection services, IConfiguration configuration)
	{
		var minioConfiguration = GetMinioConfiguration(configuration);

		services.AddSingleton<IMinioClient>(
			x => new MinioClient()
				.WithEndpoint(minioConfiguration.Endpoint)
				.WithCredentials(
					minioConfiguration.AccessKey,
					minioConfiguration.SecretKey
				)
				.Build()
			);
	}

	public static void AddSignletonConfigurations(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddSingleton(x => GetRabbitMqConfiguration(configuration));
		services.AddSingleton(x => GetMinioConfiguration(configuration));
		services.AddSingleton(x => GetProjectTrackerClientConfiguration(configuration));
	}

	private static PdfReportRabbitMqConfiguration GetRabbitMqConfiguration(IConfiguration configuration)
	{
		return configuration
			.GetRequiredSection(EnvironmentConstants.RabbitMqKey)
			.GetRequired<PdfReportRabbitMqConfiguration>();
	}

	private static MinioConfiguration GetMinioConfiguration(IConfiguration configuration) 
	{
		return configuration
			.GetRequiredSection(EnvironmentConstants.MinioKey)
			.GetRequired<MinioConfiguration>();
	}

	private static ProjectTrackerClientConfiguration GetProjectTrackerClientConfiguration(IConfiguration configuration) 
	{
		return configuration
			.GetRequiredSection(EnvironmentConstants.ProjectTrackerClientKey)
			.GetRequired<ProjectTrackerClientConfiguration>();
	}
}
