using ProjectTracker.Abstractions.ConfigurationObjects;

namespace ProjectTracker.PdfReport.ObjectStorage.Data;

//TODO Binding нужно пересмотреть
public class PdfReportRabbitMqConfiguration : RabbitMqConfigurationBase
{
	public required RabbitMqFanoutBinding DefaultEndpoint { get; set; }
	public required RabbitMqTopicBinding ReportResultTaskGroupEndpoint { get; set; }
	public required RabbitMqTopicBinding ReportResultTaskEndpoint { get; set; }
	public required RabbitMqTopicBinding ReportInputTaskGroupEndpoint { get; set; }
	public required RabbitMqTopicBinding ReportInputTaskEndpoint { get; set; }
}
