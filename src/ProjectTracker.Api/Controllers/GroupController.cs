using Microsoft.AspNetCore.Mvc;
using ProjectTracker.Api.ObjectStorage.Data.ViewModels.Shared.Result;
using ProjectTracker.Contracts.ViewModels.Shared.Pagination;
using ProjectTracker.Contracts.ViewModels.TaskGroup;
using ProjectTracker.Core.Services.Interfaces;
using AllGroupsResponse = ProjectTracker.Contracts.ViewModels.Shared.Result.MbResult<ProjectTracker.Contracts.ViewModels.Shared.Pagination.PaginationResponse<ProjectTracker.Contracts.ViewModels.TaskGroup.TaskGroupBaseResponse>>;
using GroupErrorResponse = ProjectTracker.Contracts.ViewModels.Shared.Result.MbResult<ProjectTracker.Contracts.ViewModels.TaskGroup.TaskGroupBaseResponse>;
using GroupResponse = ProjectTracker.Contracts.ViewModels.Shared.Result.MbResult<ProjectTracker.Contracts.ViewModels.TaskGroup.TaskGroupBaseResponse>;

namespace ProjectTracker.Api.Controllers;

[Route("/api/groups")]
public class GroupController(IGroupService groupService) : ControllerBase
{
	[ProducesResponseType(typeof(AllGroupsResponse), 200)]
	[HttpGet]
	public async Task<IActionResult> GetAllGroups([FromQuery] PaginationRequest request)
	{
		var result = await groupService.GetAllAsync(request);

		return Ok(MbResultFactory.WithSuccess(result));
	}

	[ProducesResponseType(typeof(GroupResponse), 200)]
	[ProducesResponseType(typeof(GroupErrorResponse), 404)]
	[HttpGet("{id:long}")]
	public async Task<IActionResult> GetGroup([FromRoute] long id)
	{
		var result = await groupService.GetAsync(id);

		return Ok(MbResultFactory.WithSuccess(result));
	}

	[ProducesResponseType(typeof(GroupResponse), 200)]
	[ProducesResponseType(typeof(GroupErrorResponse), 400)]
	[HttpPost]
	public async Task<IActionResult> CreateGroup([FromBody] CreateTaskGroupRequest request)
	{
		var result = await groupService.CreateAsync(request);

		return Ok(MbResultFactory.WithSuccess(result));
	}

	[ProducesResponseType(typeof(GroupResponse), 200)]
	[HttpPut]
	public async Task<IActionResult> UpdateGroup([FromBody] UpdateTaskGroupRequest request)
	{
		var result = await groupService.UpdateAsync(request);

		return Ok(MbResultFactory.WithSuccess(result));
	}

	[ProducesResponseType(404)]
	[HttpDelete("{id:long}")]
	public async Task<IActionResult> DeleteGroup([FromRoute] long id)
	{
		await groupService.DeleteAsync(id);

		return Ok();
	}
}