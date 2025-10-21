using Mapster;
using MassTransit;
using ProjectTracker.Abstractions.Exceptions;
using ProjectTracker.Contracts.Events.Reports;
using ProjectTracker.Contracts.ViewModels.Report;
using ProjectTracker.Core.ObjectStorage.Interfaces;
using ProjectTracker.Core.Services.Interfaces;
using ProjectTracker.Infrastructure.Enums;
using ProjectTracker.Infrastructure.Models;
using ProjectTracker.Infrastructure.Repositories.Interfaces;

namespace ProjectTracker.Core.Services;

public class ReportService(
	IRepository<ReportModel> reportRepository,
	IReportEventAwaiter reportEventAwaiter,
	IPublishEndpoint publishEndpoint) : IReportService
{
	public async Task<ReportResponse> GenerateTaskReportAsync(TaskReportRequest request)
	{
		ReportModel report = await CreateReportAsync(ReportType.Task);

		var reportEvent = new ReportInputTaskEvent
		{
			ReportId = report.Id,
			TaskId = request.TaskId,
			ExpirySeconds = request.ExpirySeconds
		};

		var result = report.Adapt<ReportResponse>();

		result.Url = await ProcessAwaiter(reportEvent, request.Timeout);

		return result;
	}

	public async Task<ReportResponse> GenerateGroupReportAsync(TaskGroupReportRequest request)
	{
		var report = await CreateReportAsync(ReportType.TaskGroup);
		var reportEvent = new ReportInputTaskGroupEvent
		{
			ReportId = report.Id,
			TaskGroupId = request.GroupId,
			ExpirySeconds = request.ExpirySeconds
		};

		var result = report.Adapt<ReportResponse>();

		result.Url = await ProcessAwaiter(reportEvent, request.Timeout);

		return result;
	}

	public async Task ProcessReportResultAsync(ReportResultEvent resultEvent)
	{
		var reportModel = await reportRepository.FindAsync(resultEvent.ReportId);

		reportModel.Status = ReportStatus.Completed;
		reportModel.Url = resultEvent.Url;

		await reportRepository.UpdateAsync(reportModel);
	}

	public async Task ProcessReportErrorAsync(Guid reportId)
	{
		var reportModel = await reportRepository.FindAsync(reportId);

		reportModel.Status = ReportStatus.Failed;

		await reportRepository.UpdateAsync(reportModel);
	}

	private async Task<ReportModel> CreateReportAsync(ReportType type)
	{
		var report = new ReportModel
		{
			Id = Guid.NewGuid(),
			Type = type,
			Status = ReportStatus.Default
		};

		await reportRepository.AddAsync(report);

		return report;
	}

	private async Task<string?> ProcessAwaiter<TEvent>(TEvent reportEvent, TimeSpan? timeout) where TEvent : ReportInputEventBase
	{
		if (timeout is { })
		{
			reportEventAwaiter.AddReportId(reportEvent.ReportId);
		}

		await publishEndpoint.Publish(reportEvent);

		if (timeout is { })
		{
			try
			{
				return await reportEventAwaiter.WaitEvent(reportEvent.ReportId, timeout);
			}
			catch (TimeoutException)
			{
				throw new UnprocessableException($"Превышено время ожидания");
			}
			catch (Exception)
			{
				throw;
			}
		}

		return null;
	}

	public async Task<ReportResponse> GetReportAsync(GetReportRequest request)
	{
		var model = await reportRepository.FindAsync(request.Id);

		return model.Adapt<ReportResponse>();
	}
}
