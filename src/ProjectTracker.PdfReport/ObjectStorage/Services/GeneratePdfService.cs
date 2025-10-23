using Mapster;
using ProjectTracker.Abstractions.Exceptions;
using ProjectTracker.Contracts.Events.Reports;
using ProjectTracker.PdfReport.ObjectStorage.Data;
using ProjectTracker.PdfReport.ObjectStorage.Dtos.TaskGroupReport;
using ProjectTracker.PdfReport.ObjectStorage.Dtos.TaskReport;
using ProjectTracker.PdfReport.ObjectStorage.Reports;
using ProjectTracker.PdfReport.ObjectStorage.Reports.TaskGroupReport;
using ProjectTracker.PdfReport.ObjectStorage.Services.Interfaces;
using QuestPDF.Fluent;

namespace ProjectTracker.PdfReport.ObjectStorage.Services;

public class GeneratePdfService(
	MinioConfiguration minioConfiguration,
	IProjectTrackerClient projectTrackerClient,
	IMinioService minioService) : IGeneratePdfService
{
	public async Task<string> GenerateTaskReportAsync(ReportInputTaskEvent inputEvent)
	{
		var task = (await projectTrackerClient.GetTaskOrDefaultAsync(inputEvent.TaskId))
			?.Result
			?.Adapt<TaskReportDto>()
			?? throw new NotFoundException($"Не удалось найти задачу с id = {inputEvent.TaskId}");

		var document = new TaskReport(task);

		var bytes = document.GeneratePdf();

		var objectName = $"task-report-{inputEvent.TaskId}_{inputEvent.ReportId}";

		await minioService.UploadFileAsync(minioConfiguration.ReportBucket, objectName, bytes);

		var url = await minioService.GetFileUrl(minioConfiguration.ReportBucket, objectName, inputEvent.ExpirySeconds);

		return url;
	}

	public async Task<string> GenerateTaskGroupReportAsync(ReportInputTaskGroupEvent inputEvent)
	{
		var taskGroup = (await projectTrackerClient.GetTaskGroupOrDefaultAsync(inputEvent.TaskGroupId))
			?.Result
			?.Adapt<TaskGroupReportDto>()
			?? throw new NotFoundException($"Не удалось найти группу задач с id = {inputEvent.TaskGroupId}");

		var document = new TaskGroupReport(taskGroup);

		var bytes = document.GeneratePdf();

		var objectName = $"task-group-report-{inputEvent.TaskGroupId}_{inputEvent.ReportId}";

		await minioService.UploadFileAsync(minioConfiguration.ReportBucket, objectName, bytes);

		var url = await minioService.GetFileUrl(minioConfiguration.ReportBucket, objectName, inputEvent.ExpirySeconds);

		return url;
	}
}


