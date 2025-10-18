using ProjectTracker.Contracts.ViewModels.Shared.Pagination;
using ProjectTracker.Contracts.ViewModels.Shared.Result;
using ProjectTracker.Contracts.ViewModels.Task;
using ProjectTracker.Contracts.ViewModels.TaskGroup;

namespace ProjectTracker.PdfReport.ObjectStorage.Services.Interfaces;

public interface IProjectTrackerClient
{
	Task<MbResult<TaskReportInformationResponse>?> GetTaskOrDefaultAsync(
		long taskId,
		CancellationToken cancellationToken = default);

	Task<MbResult<TaskGroupInformationResponse>?> GetTaskGroupOrDefaultAsync(
		long taskGroupId,
		CancellationToken cancellationToken = default);
}