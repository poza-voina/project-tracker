using ProjectTracker.Contracts.ViewModels.Project;
using ProjectTracker.Contracts.ViewModels.Shared.Pagination;

namespace ProjectTracker.Core.Services.Interfaces;

public interface IProjectService
{
	Task<ProjectBaseResponse> CreateAsync(CreateProjectRequest request);
	Task DeleteAsync(DeleteProjectRequest id);
	Task<PaginationResponse<ProjectBaseResponse>> GetAllAsync(PaginationRequest request);
	Task<ProjectBaseResponse> GetAsync(GetProjectRequest request);
	Task<ProjectBaseResponse> UpdateAsync(UpdateProjectRequest request);
}
