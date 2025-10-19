using MassTransit;
using ProjectTracker.Contracts.Events.Reports;
using ProjectTracker.Core.ObjectStorage.Interfaces;
using ProjectTracker.Core.Services.Interfaces;

namespace ProjectTracker.Api.ObjectStorage.Consumers;

public class ReportErrorConsumer(
	IReportEventAwaiter awaiter,
	ILogger<ReportErrorEvent> logger,
	IReportService reportService
	) : IConsumer<ReportErrorEvent>
{
	public async Task Consume(ConsumeContext<ReportErrorEvent> context)
	{
		logger.LogInformation("event received {data}", context.Message);

		try
		{
			awaiter.ProcessErrorEvent(context.Message);
		}
		catch (Exception)
		{
		}

		await reportService.ProcessReportErrorAsync(context.Message.ReportId);
		
	}
}
