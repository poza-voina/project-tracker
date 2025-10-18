using MassTransit;
using ProjectTracker.Contracts.Events.Reports;
using ProjectTracker.Core.ObjectStorage.Interfaces;

namespace ProjectTracker.Api.ObjectStorage.Consumers;

public class ReportErrorConsumer(IReportEventAwaiter awaiter, ILogger<ReportErrorEvent> logger) : IConsumer<ReportErrorEvent>
{
	public Task Consume(ConsumeContext<ReportErrorEvent> context)
	{
		logger.LogInformation("event received {data}", context.Message);

		awaiter.ProcessErrorEvent(context.Message);

		return Task.CompletedTask;
	}
}
