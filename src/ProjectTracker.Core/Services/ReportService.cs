using MassTransit;
using ProjectTracker.Abstractions.ConfigurationObjects;
using ProjectTracker.Contracts.Events.PublishEvents.Shared;
using ProjectTracker.Contracts.Events.Reports;
using ProjectTracker.Contracts.ViewModels.Report;
using ProjectTracker.Core.ObjectStorage.Interfaces;
using ProjectTracker.Core.Services.Interfaces;

namespace ProjectTracker.Core.Services;

public class ReportService(
	IReportEventAwaiter reportEventAwaiter,
	IEventPublisher publisher,
	ProjectTrackerRabbitMqConfiguration rabbitMqConfiguration
	) : IReportService
{
	public async Task<ReportResponse> GenerateTaskReportAsync(long taskId)
	{
		var reportId = Guid.NewGuid();
		var reportEvent = new ReportInputTaskEvent
		{
			ReportId = reportId,
			TaskId = taskId
		};

		reportEventAwaiter.AddReportId(reportId);

		await publisher.Publish(
			EventWrapper<object>.Wrap(reportEvent),
			rabbitMqConfiguration.ReportInputEndpoint.RoutingKey,
			rabbitMqConfiguration.DefaultEndpoint.Name);

		var fileUrl = await reportEventAwaiter.WaitEvent(reportId);

		return new ReportResponse { Url = fileUrl };
	}

	public async Task<ReportResponse> GenerateGroupReportAsync(long groupId)
	{
		var reportId = Guid.NewGuid();
		
		var reportEvent = new ReportInputTaskGroupEvent
		{
			ReportId = reportId,
			TaskGroupId = groupId
		};

		reportEventAwaiter.AddReportId(reportId);

		await publisher.Publish(
			EventWrapper<object>.Wrap(reportEvent),
			rabbitMqConfiguration.ReportInputEndpoint.RoutingKey,
			rabbitMqConfiguration.DefaultEndpoint.Name);

		var fileUrl = await reportEventAwaiter.WaitEvent(reportId);

		return new ReportResponse { Url = fileUrl };
	}
}
