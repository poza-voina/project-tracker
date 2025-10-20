using Microsoft.AspNetCore.Mvc;
using ProjectTracker.Api.ObjectStorage.Data.ViewModels.Shared.Result;
using ProjectTracker.Contracts.ViewModels.Project;
using ProjectTracker.Contracts.ViewModels.Shared.Pagination;
using ProjectTracker.Core.Services.Interfaces;
using AllProjectsResponse = ProjectTracker.Contracts.ViewModels.Shared.Result.MbResult<ProjectTracker.Contracts.ViewModels.Shared.Pagination.PaginationResponse<ProjectTracker.Contracts.ViewModels.Project.ProjectBaseResponse>>;
using ProjectErrorResponse = ProjectTracker.Contracts.ViewModels.Shared.Result.MbResult<ProjectTracker.Contracts.ViewModels.Project.ProjectBaseResponse>;
using ProjectResponse = ProjectTracker.Contracts.ViewModels.Shared.Result.MbResult<ProjectTracker.Contracts.ViewModels.Project.ProjectBaseResponse>;

namespace ProjectTracker.Api.Controllers;

[Route("/api/projects")]
public class ProjectController(IProjectService projectService) : ControllerBase
{
	[ProducesResponseType(typeof(AllProjectsResponse), 200)]
	[ProducesResponseType(typeof(ProjectErrorResponse), 400)]
	[HttpGet]
	public async Task<IActionResult> GetAllProjects([FromQuery] PaginationRequest request)
	{
		var result = await projectService.GetAllAsync(request);

		return Ok(MbResultFactory.WithSuccess(result));
	}

	[ProducesResponseType(typeof(ProjectResponse), 200)]
	[ProducesResponseType(typeof(ProjectErrorResponse), 404)]
	[HttpGet("{id:long}")]
	public async Task<IActionResult> GetProject([FromRoute] long id)
	{
		var result = await projectService.GetAsync(id);

		return Ok(MbResultFactory.WithSuccess(result));
	}

	[ProducesResponseType(typeof(ProjectResponse), 200)]
	[ProducesResponseType(typeof(ProjectErrorResponse), 400)]
	[ProducesResponseType(typeof(ProjectErrorResponse), 422)]
	[HttpPost]
	public async Task<IActionResult> CreateProject([FromBody] CreateProjectRequest request)
	{
		var result = await projectService.CreateAsync(request);

		return Ok(MbResultFactory.WithSuccess(result));
	}

	[ProducesResponseType(typeof(ProjectResponse), 200)]
	[ProducesResponseType(typeof(ProjectErrorResponse), 400)]
	[ProducesResponseType(typeof(ProjectErrorResponse), 422)]
	[ProducesResponseType(typeof(ProjectErrorResponse), 404)]
	[HttpPut]
	public async Task<IActionResult> UpdateProject([FromBody] UpdateProjectRequest request)
	{
		var result = await projectService.UpdateAsync(request);

		return Ok(MbResultFactory.WithSuccess(result));
	}

	[ProducesResponseType(404)]
	[ProducesResponseType(typeof(ProjectErrorResponse), 400)]
	[HttpDelete("{id:long}")]
	public async Task<IActionResult> DeleteProject([FromRoute] long id)
	{
		await projectService.DeleteAsync(id);

		return Ok();
	}
}