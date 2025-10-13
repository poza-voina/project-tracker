using ProjectTracker.Contracts.ViewModels.Shared.Pagination;
using ProjectTracker.Contracts.ViewModels.TaskGroup;

namespace ProjectTracker.Core.Services.Interfaces;

public interface IGroupService
{
	Task<TaskGroupBaseResponse> CreateAsync(CreateTaskGroupRequest request);
	Task DeleteAsync(long id);
	Task<PaginationResponse<TaskGroupBaseResponse>> GetAllAsync(PaginationRequest request);
	Task<TaskGroupBaseResponse> GetAsync(long id);
	Task<TaskGroupBaseResponse> UpdateAsync(UpdateTaskGroupRequest request);
}