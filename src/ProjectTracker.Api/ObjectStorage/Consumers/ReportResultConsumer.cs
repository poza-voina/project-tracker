using MassTransit;
using ProjectTracker.Contracts.Events.Reports;
using ProjectTracker.Core.ObjectStorage.Interfaces;
using ProjectTracker.Core.Services.Interfaces;

namespace ProjectTracker.Api.ObjectStorage.Consumers;

public class ReportResultConsumer(
	IReportEventAwaiter awaiter,
	ILogger<ReportResultConsumer> logger,
	IReportService reportService) : IConsumer<ReportResultEvent>
{
	public async Task Consume(ConsumeContext<ReportResultEvent> context)
	{
		logger.LogInformation("event received {data}", context.Message);

		try
		{
			awaiter.ProcessResultEvent(context.Message);
		}
		catch (Exception)
		{
		}

		await reportService.ProcessReportResultAsync(context.Message);
	}
}