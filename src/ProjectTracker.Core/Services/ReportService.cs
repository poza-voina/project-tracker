using MassTransit;
using ProjectTracker.Abstractions.Exceptions;
using ProjectTracker.Contracts.Events.Reports;
using ProjectTracker.Contracts.ViewModels.Report;
using ProjectTracker.Core.ObjectStorage.Interfaces;
using ProjectTracker.Core.Services.Interfaces;

namespace ProjectTracker.Core.Services;

public class ReportService(
	IReportEventAwaiter reportEventAwaiter,
	IPublishEndpoint publishEndpoint) : IReportService
{
	public async Task<ReportResponse> GenerateTaskReportAsync(TaskReportRequest request)
	{
		var reportId = Guid.NewGuid();
		var reportEvent = new ReportInputTaskEvent
		{
			ReportId = reportId,
			TaskId = request.TaskId,
			ExpirySeconds = request.ExpirySeconds
		};

		reportEventAwaiter.AddReportId(reportId);

		await publishEndpoint.Publish(reportEvent);

		var fileUrl = await reportEventAwaiter.WaitEvent(reportId)
			?? throw new UnprocessableException($"Не удалось сформировать отчет reportId = {reportId} для задачи c id = {request.TaskId}");

		return new ReportResponse { Url = fileUrl };
	}

	public async Task<ReportResponse> GenerateGroupReportAsync(TaskGroupReportRequest request)
	{
		var reportId = Guid.NewGuid();

		var reportEvent = new ReportInputTaskGroupEvent
		{
			ReportId = reportId,
			TaskGroupId = request.GroupId,
			ExpirySeconds = request.ExpirySeconds
		};

		reportEventAwaiter.AddReportId(reportId);

		await publishEndpoint.Publish(reportEvent);

		var fileUrl = await reportEventAwaiter.WaitEvent(reportId)
			?? throw new UnprocessableException($"Не удалось сформировать отчет reportId = {reportId} для групп задач c id = {request.GroupId}");

		return new ReportResponse { Url = fileUrl };
	}
}
