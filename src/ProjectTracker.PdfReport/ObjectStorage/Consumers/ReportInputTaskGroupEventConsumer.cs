using MassTransit;
using Microsoft.Extensions.Logging;
using ProjectTracker.Abstractions.Exceptions;
using ProjectTracker.Contracts.Events.Reports;
using ProjectTracker.PdfReport.ObjectStorage.Services.Interfaces;

namespace ProjectTracker.PdfReport.ObjectStorage.Consumers;

public class ReportInputTaskGroupEventConsumer(
	IGeneratePdfService generatePdfService,
	ILogger<ReportInputTaskGroupEventConsumer> logger,
	IPublishEndpoint publishEndpoint) : IConsumer<ReportInputTaskGroupEvent>
{
	public async Task Consume(ConsumeContext<ReportInputTaskGroupEvent> context)
	{
		var message = context.Message;
		try
		{
			var url = await generatePdfService.GenerateTaskGroupReportAsync(message);

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