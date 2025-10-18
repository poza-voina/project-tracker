
namespace ProjectTracker.Abstractions.ConfigurationObjects;

//TODO RabbitMqTopicBinding переделать уже другой смысл несет
public class ProjectTrackerRabbitMqConfiguration : RabbitMqConfigurationBase
{
	public required RabbitMqTopicBinding ReportErrorEndpoint { get; set; }
	public required RabbitMqTopicBinding ReportResultEndpoint { get; set; }
	public required RabbitMqTopicBinding ReportInputTaskGroupEndpoint { get; set; }
	public required RabbitMqTopicBinding ReportInputTaskEndpoint { get; set; }
	public required RabbitMqTopicBinding HistoryEndpoint { get; set; }
	public required RabbitMqFanoutBinding DefaultEndpoint { get; set; }
}