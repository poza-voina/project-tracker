using ProjectTracker.Abstractions.ConfigurationObjects;

namespace ProjectTracker.PdfReport.ObjectStorage.Data;

public class PdfReportRabbitMqConfiguration : RabbitMqConfigurationBase
{
	public required RabbitMqEndpointBinding ReportResultTaskGroupEndpoint { get; set; }
	public required RabbitMqEndpointBinding ReportResultTaskEndpoint { get; set; }
	public required RabbitMqEndpointBinding ReportInputTaskGroupEndpoint { get; set; }
	public required RabbitMqEndpointBinding ReportInputTaskEndpoint { get; set; }
}
