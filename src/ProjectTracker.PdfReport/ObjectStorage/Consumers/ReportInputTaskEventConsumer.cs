using MassTransit;
using ProjectTracker.Abstractions.Exceptions;
using ProjectTracker.Contracts.Events.Reports;
using ProjectTracker.PdfReport.ObjectStorage.Services.Interfaces;

namespace ProjectTracker.PdfReport.ObjectStorage.Consumers;

public class ReportInputTaskEventConsumer(
	IGeneratePdfService generatePdfService,
	IPublishEndpoint publishEndpoint) : IConsumer<ReportInputTaskEvent>
{
	public async Task Consume(ConsumeContext<ReportInputTaskEvent> context)
	{
		var message = context.Message;
		try
		{
			var url = await generatePdfService.GenerateTaskReportAsync(message);

			var @event = new ReportResultEvent
			{
				ReportId = message.ReportId,
				Url = url,
			};

			await publishEndpoint.Publish(@event);
		}
		catch (NotFoundException ex)
		{
			await publishEndpoint.Publish(
				new ReportErrorEvent
				{
					ReportId = message.ReportId,
					Message = ex.Message
				});
		}
	}
}