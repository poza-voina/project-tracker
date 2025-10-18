
namespace ProjectTracker.Abstractions.ConfigurationObjects;

public class ProjectTrackerRabbitMqConfiguration : RabbitMqConfigurationBase
{
	public required RabbitMqEndpointBinding ReportErrorEndpoint { get; set; }
	public required RabbitMqEndpointBinding ReportResultEndpoint { get; set; }
	public required RabbitMqEndpointBinding ReportInputTaskGroupEndpoint { get; set; }
	public required RabbitMqEndpointBinding ReportInputTaskEndpoint { get; set; }
	public required RabbitMqEndpointBinding HistoryEndpoint { get; set; }
}