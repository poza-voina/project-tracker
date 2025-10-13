using Microsoft.AspNetCore.Mvc;
using ProjectTracker.Api.ObjectStorage.Data.ViewModels.Shared.Result;
using ProjectTracker.Contracts.ViewModels.Task;
using ProjectTracker.Core.Services.Interfaces;
using AllTaskWithEmployesResponse = ProjectTracker.Contracts.ViewModels.Shared.Result.MbResult<ProjectTracker.Contracts.ViewModels.Shared.Pagination.PaginationResponse<ProjectTracker.Contracts.ViewModels.Task.TaskWithStatusEmployeesReponse>>;
using TaskErrorResponse = ProjectTracker.Contracts.ViewModels.Shared.Result.MbResult<ProjectTracker.Contracts.ViewModels.Task.TaskBaseReponse>;
using TaskResponse = ProjectTracker.Contracts.ViewModels.Shared.Result.MbResult<ProjectTracker.Contracts.ViewModels.Task.TaskWithStatusResponse>;
using TaskWithEmployesResponse = ProjectTracker.Contracts.ViewModels.Shared.Result.MbResult<ProjectTracker.Contracts.ViewModels.Task.TaskWithStatusEmployeesReponse>;

namespace ProjectTracker.Api.Controllers;

[Route("api/tasks")]
public class TaskController(ITaskService taskService) : ControllerBase
{
	[ProducesResponseType(typeof(TaskWithEmployesResponse), 200)]
	[ProducesResponseType(typeof(TaskErrorResponse), 404)]
	[HttpGet("{id:long}")]
	public async Task<IActionResult> GetTask([FromRoute] long id)
	{
		var result = await taskService.GetAsync(id);

		return Ok(MbResultFactory.WithSuccess(result));
	}

	[ProducesResponseType(typeof(AllTaskWithEmployesResponse), 200)]
	[HttpGet]
	public async Task<IActionResult> GetAllTask([FromQuery] GetPaginationTasksRequest request)
	{
		var result = await taskService.GetAllAsync(request);

		return Ok(MbResultFactory.WithSuccess(result));
	}

	[ProducesResponseType(typeof(TaskResponse), 200)]
	[ProducesResponseType(typeof(TaskErrorResponse), 400)]
	[HttpPost]
	public async Task<IActionResult> CreateTask([FromBody] CreateTaskRequest request)
	{
		var result = await taskService.CreateAsync(request);

		return Ok(MbResultFactory.WithSuccess(result));
	}

	[ProducesResponseType(404)]
	[HttpDelete("{id:long}")]
	public async Task<IActionResult> DeleteTaskAsync([FromRoute] long id)
	{
		await taskService.DeleteAsync(id);

		return Ok();
	}

	[HttpPatch("change-status")]
	public async Task<IActionResult> ChangeStatus([FromBody] ChangeTaskStatusRequest request)
	{
		var result = await taskService.ChangeStatusAsync(request);

		return Ok(MbResultFactory.WithSuccess(result));
	}

	[HttpPost("add-performer")]
	public async Task<IActionResult> AddPerformer([FromBody] AddTaskPerformerRequest request)
	{
		await taskService.AddPerformerAsync(request);

		return Ok();
	}

	[HttpPost("add-observer")]
	public async Task<IActionResult> AddObserver([FromBody] AddTaskObserverRequest request)
	{
		await taskService.AddObserverAsync(request);

		return Ok();
	}
}
