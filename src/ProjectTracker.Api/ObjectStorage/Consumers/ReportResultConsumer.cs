using MassTransit;
using ProjectTracker.Contracts.Events.PublishEvents.Shared;
using ProjectTracker.Contracts.Events.Reports;
using ProjectTracker.Core.ObjectStorage.Interfaces;

namespace ProjectTracker.Api.ObjectStorage.Consumers;

public class ReportResultConsumer(IReportEventAwaiter awaiter, ILogger<ReportResultConsumer> logger) : IConsumer<EventWrapper<ReportResultEvent>>
{
	public Task Consume(ConsumeContext<EventWrapper<ReportResultEvent>> context)
	{
		logger.LogInformation("event received {data}", context.Message);

		awaiter.ProcessResultEvent(context.Message.Event);

		return Task.CompletedTask;
	}
}
