
namespace ProjectTracker.Abstractions.ConfigurationObjects;

public class ProjectTrackerRabbitMqConfiguration : RabbitMqConfigurationBase
{
	public required RabbitMqTopicBinding ReportResultEndpoint { get; set; }
	public required RabbitMqTopicBinding ReportInputEndpoint { get; set; }
	public required RabbitMqTopicBinding HistoryEndpoint { get; set; }
	public required RabbitMqFanoutBinding DefaultEndpoint { get; set; }
}