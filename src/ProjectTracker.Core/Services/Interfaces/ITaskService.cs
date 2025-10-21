
using ProjectTracker.Contracts.ViewModels.Report;
using ProjectTracker.Contracts.ViewModels.Shared.Pagination;
using ProjectTracker.Contracts.ViewModels.Task;

namespace ProjectTracker.Core.Services.Interfaces;

public interface ITaskService
{
	Task<TaskWithStatusEmployeesReponse> GetAsync(GetTaskRequest request);
	Task<PaginationResponse<TaskWithStatusEmployeesReponse>> GetAllAsync(GetPaginationTasksRequest request);
	Task<TaskWithStatusResponse> CreateAsync(CreateTaskRequest request);
	Task<TaskWithStatusResponse> UpdateAsync(UpdateTaskRequest request);
	Task DeleteAsync(DeleteTaskRequest request);
	Task<TaskWithStatusEmployeesReponse> ChangeStatusAsync(ChangeTaskStatusRequest request);
	Task AddPerformerAsync(AddTaskPerformerRequest request);
	Task AddObserverAsync(AddTaskObserverRequest request);
	Task<TaskReportInformationResponse> GetReportInformationAsync(long taskId);
}
