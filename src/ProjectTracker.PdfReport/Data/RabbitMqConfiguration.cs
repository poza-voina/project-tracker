using ProjectTracker.Abstractions.ConfigurationObjects;

namespace ProjectTracker.PdfReport.Data;

public class PdfReportRabbitMqConfiguration : RabbitMqConfigurationBase
{
	public required RabbitMqFanoutBinding DefaultEndpoint { get; set; }
	public required RabbitMqTopicBinding ReportResultEndpoint { get; set; }
	public required RabbitMqTopicBinding ReportInputTaskGroupEventEndpoint { get; set; }
	public required RabbitMqTopicBinding ReportInputTaskEventEndpoint { get; set; }
	public required RabbitMqTopicBinding ReportInputEndpoint { get; set; }
}
