using Microsoft.AspNetCore.Mvc;
using ProjectTracker.History.Contracts.ViewModels.TaskHistory;
using ProjectTracker.History.Core.Services.Interfaces;

namespace ProjectTracker.History.Api.Controllers;

[Route("tasks")]
public class TaskHistoryController(ITaskHistoryService historyService) : ControllerBase
{
	[HttpGet]
	public async Task<IActionResult> GetAllTaskHistories([FromQuery] GetTasksHistoryRequest request)
	{
		var result = await historyService.GetAllAsync(request);

		return Ok(result);
	}
}
