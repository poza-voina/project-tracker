using MassTransit;
using ProjectTracker.Contracts.Events.Reports;
using ProjectTracker.PdfReport.ObjectStorage.Services.Interfaces;

namespace ProjectTracker.PdfReport.ObjectStorage.Consumers;

public class ReportInputTaskEventConsumer(
	IGeneratePdfService generatePdfService,
	ILogger<ReportInputTaskEventConsumer> logger,
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
		catch (Exception ex)
		{
			logger.LogError("Ошибка создания отчета id = {reportId} Exception = {ex}", message.ReportId, ex);

			await publishEndpoint.Publish(
				new ReportErrorEvent
				{
					ReportId = message.ReportId,
					Message = ex.Message
				});
		}
	}
}