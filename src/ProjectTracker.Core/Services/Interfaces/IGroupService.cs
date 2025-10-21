using ProjectTracker.Contracts.ViewModels.Shared.Pagination;
using ProjectTracker.Contracts.ViewModels.TaskGroup;

namespace ProjectTracker.Core.Services.Interfaces;

public interface IGroupService
{
	Task<TaskGroupBaseResponse> CreateAsync(CreateTaskGroupRequest request);
	Task DeleteAsync(DeleteGroupRequest request);
	Task<PaginationResponse<TaskGroupBaseResponse>> GetAllAsync(PaginationRequest request);
	Task<TaskGroupBaseResponse> GetAsync(GetGroupRequest request);
	Task<TaskGroupInformationResponse> GetReportInformationAsync(GetGroupReportInforamationRequest request);
	Task<TaskGroupBaseResponse> UpdateAsync(UpdateTaskGroupRequest request);
}