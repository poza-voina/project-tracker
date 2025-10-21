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
	[ProducesResponseType(typeof(GroupErrorResponse), 400)]
	[HttpGet]
	public async Task<IActionResult> GetAllGroups([FromQuery] PaginationRequest request)
	{
		var result = await groupService.GetAllAsync(request);

		return Ok(MbResultFactory.WithSuccess(result));
	}

	[ProducesResponseType(typeof(GroupResponse), 200)]
	[ProducesResponseType(typeof(GroupErrorResponse), 404)]
	[HttpGet("{Id:long}")]
	public async Task<IActionResult> GetGroup([FromRoute] GetGroupRequest request)
	{
		var result = await groupService.GetAsync(request);

		return Ok(MbResultFactory.WithSuccess(result));
	}

	[ProducesResponseType(typeof(GroupResponse), 200)]
	[ProducesResponseType(typeof(GroupErrorResponse), 400)]
	[ProducesResponseType(typeof(GroupErrorResponse), 422)]
	[HttpPost]
	public async Task<IActionResult> CreateGroup([FromBody] CreateTaskGroupRequest request)
	{
		var result = await groupService.CreateAsync(request);

		return Ok(MbResultFactory.WithSuccess(result));
	}

	[ProducesResponseType(typeof(GroupResponse), 200)]
	[ProducesResponseType(typeof(GroupErrorResponse), 404)]
	[ProducesResponseType(typeof(GroupErrorResponse), 422)]
	[HttpPut]
	public async Task<IActionResult> UpdateGroup([FromBody] UpdateTaskGroupRequest request)
	{
		var result = await groupService.UpdateAsync(request);

		return Ok(MbResultFactory.WithSuccess(result));
	}

	[ProducesResponseType(200)]
	[ProducesResponseType(typeof(GroupErrorResponse), 404)]
	[ProducesResponseType(typeof(GroupErrorResponse), 422)]
	[HttpDelete("{Id:long}")]
	public async Task<IActionResult> DeleteGroup([FromRoute] DeleteGroupRequest request)
	{
		await groupService.DeleteAsync(request);

		return Ok();
	}

	[ProducesResponseType(typeof(TaskGroupInformationResponse), 200)]
	[ProducesResponseType(typeof(GroupErrorResponse), 404)]
	[HttpGet("informations/{id:long}")]
	public async Task<IActionResult> GetGroupReportInforamation([FromRoute] GetGroupReportInforamationRequest request)
	{
		var result = await groupService.GetReportInformationAsync(request);

		return Ok(MbResultFactory.WithSuccess(result));
	}
}