
namespace ProjectTracker.Abstractions.ConfigurationObjects;

public class ProjectTrackerRabbitMqConfiguration : RabbitMqConfigurationBase
{
	public required RabbitMqTopicBinding ReportErrorEndpoint { get; set; }
	public required RabbitMqTopicBinding ReportResultEndpoint { get; set; }
	public required RabbitMqTopicBinding ReportInputTaskGroupEndpoint { get; set; }
	public required RabbitMqTopicBinding ReportInputTaskEndpoint { get; set; }
	public required RabbitMqTopicBinding HistoryEndpoint { get; set; }
	public required RabbitMqFanoutBinding DefaultEndpoint { get; set; }
}