using MassTransit;
using ProjectTracker.Contracts.Events.Reports;
using ProjectTracker.PdfReport.Services.Interfaces;

namespace ProjectTracker.PdfReport.Data;

public class ReportInputTaskEventConsumer(
	IGeneratePdfService generatePdfService,
	IPublishEndpoint endpoint) : IConsumer<ReportInputTaskEvent>
{
	public async Task Consume(ConsumeContext<ReportInputTaskEvent> context) //TODO доделать
	{
		//logger.LogInformation("event received {data}", context.Message);
		//var url = await generatePdfService.GenerateTaskReport(context.Message);

		var @event = new ReportResultEvent
		{
			ReportId = context.Message.ReportId,
			Url = "TEMP URL" //TODO сделать
		};

		await endpoint.Publish(@event);
	}
}
