using Microsoft.AspNetCore.Mvc;
using ProjectTracker.Api.ObjectStorage.Data.ViewModels.Shared.Result;
using ProjectTracker.Contracts.ViewModels.Task;
using ProjectTracker.Core.Services.Interfaces;
using TaskErrorResponse = ProjectTracker.Contracts.ViewModels.Shared.Result.MbResult<object>;
using TaskReportResponse = ProjectTracker.Contracts.ViewModels.Shared.Result.MbResult<ProjectTracker.Contracts.ViewModels.Task.TaskReportInformationResponse>;
using TaskWithEmployeesResponse = ProjectTracker.Contracts.ViewModels.Shared.Result.MbResult<ProjectTracker.Contracts.ViewModels.Task.TaskWithStatusEmployeesReponse>;
using TaskWithStatusResponse = ProjectTracker.Contracts.ViewModels.Shared.Result.MbResult<ProjectTracker.Contracts.ViewModels.Task.TaskWithStatusResponse>;
namespace ProjectTracker.Api.Controllers;

[Route("api/tasks")]
public class TaskController(ITaskService taskService) : ControllerBase
{
	[ProducesResponseType(typeof(TaskWithEmployeesResponse), 200)]
	[ProducesResponseType(typeof(TaskErrorResponse), 404)]
	[HttpGet("{id:long}")]
	public async Task<IActionResult> GetTask([FromRoute] long id)
	{
		var result = await taskService.GetAsync(id);

		return Ok(MbResultFactory.WithSuccess(result));
	}

	[ProducesResponseType(typeof(TaskWithEmployeesResponse), 200)]
	[ProducesResponseType(typeof(TaskErrorResponse), 400)]
	[ProducesResponseType(typeof(TaskErrorResponse), 422)]
	[HttpGet]
	public async Task<IActionResult> GetAllTask([FromQuery] GetPaginationTasksRequest request)
	{
		var result = await taskService.GetAllAsync(request);

		return Ok(MbResultFactory.WithSuccess(result));
	}

	[ProducesResponseType(typeof(TaskWithStatusResponse), 200)]
	[ProducesResponseType(typeof(TaskErrorResponse), 400)]
	[ProducesResponseType(typeof(TaskErrorResponse), 422)]
	[HttpPost]
	public async Task<IActionResult> CreateTask([FromBody] CreateTaskRequest request)
	{
		var result = await taskService.CreateAsync(request);

		return Ok(MbResultFactory.WithSuccess(result));
	}

	[ProducesResponseType(typeof(TaskWithEmployeesResponse), 200)]
	[ProducesResponseType(typeof(TaskErrorResponse), 400)]
	[ProducesResponseType(typeof(TaskErrorResponse), 404)]
	[ProducesResponseType(typeof(TaskErrorResponse), 422)]
	[HttpDelete("{id:long}")]
	public async Task<IActionResult> DeleteTaskAsync([FromRoute] long id)
	{
		await taskService.DeleteAsync(id);

		return Ok();
	}

	[ProducesResponseType(200)]
	[ProducesResponseType(typeof(TaskErrorResponse), 422)]
	[HttpPatch("change-status")]
	public async Task<IActionResult> ChangeStatus([FromBody] ChangeTaskStatusRequest request)
	{
		var result = await taskService.ChangeStatusAsync(request);

		return Ok(MbResultFactory.WithSuccess(result));
	}

	[ProducesResponseType(200)]
	[ProducesResponseType(typeof(TaskErrorResponse), 422)]
	[HttpPost("add-performer")]
	public async Task<IActionResult> AddPerformer([FromBody] AddTaskPerformerRequest request)
	{
		await taskService.AddPerformerAsync(request);

		return Ok();
	}


	[ProducesResponseType(200)]
	[ProducesResponseType(typeof(TaskErrorResponse), 422)]
	[HttpPost("add-observer")]
	public async Task<IActionResult> AddObserver([FromBody] AddTaskObserverRequest request)
	{
		await taskService.AddObserverAsync(request);

		return Ok();
	}
	
	[HttpPut]
	public async Task<IActionResult> UpdateTask([FromBody] UpdateTaskRequest request)
	{
		var result = await taskService.UpdateAsync(request);

		return Ok(MbResultFactory.WithSuccess(result));
	}

	[ProducesResponseType(typeof(TaskReportResponse), 200)]
	[ProducesResponseType(typeof(TaskErrorResponse), 400)]
	[ProducesResponseType(typeof(TaskErrorResponse), 404)]
	[ProducesResponseType(typeof(TaskErrorResponse), 422)]
	[HttpGet("informations/{taskId:long}")]
	public async Task<IActionResult> GetTaskReportInformation([FromRoute] long taskId)
	{
		var response = await taskService.GetReportInformationAsync(taskId);

		return Ok(MbResultFactory.WithSuccess(response));
	}
}
