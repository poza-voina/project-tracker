using ProjectTracker.Contracts.ViewModels.Project;
using ProjectTracker.Contracts.ViewModels.Shared.Pagination;

namespace ProjectTracker.Core.Services.Interfaces;

public interface IProjectService
{
	Task<ProjectBaseResponse> CreateAsync(CreateProjectRequest request);
	Task DeleteAsync(long id);
	Task<PaginationResponse<ProjectBaseResponse>> GetAllAsync(PaginationRequest request);
	Task<ProjectBaseResponse> GetAsync(long id);
	Task<ProjectBaseResponse> UpdateAsync(UpdateProjectRequest request);
}
