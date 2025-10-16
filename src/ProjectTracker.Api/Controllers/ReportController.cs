using Microsoft.AspNetCore.Mvc;
using ProjectTracker.Api.ObjectStorage.Data.ViewModels.Shared.Result;
using ProjectTracker.Contracts.Events.Reports;
using ProjectTracker.Contracts.ViewModels.Report;
using ProjectTracker.Contracts.ViewModels.Shared.Result;
using ProjectTracker.Core.Services.Interfaces;

namespace ProjectTracker.Api.Controllers;

[Route("api/reports")]
public class ReportController(IReportService reportService) : ControllerBase
{
	[ProducesResponseType(typeof(MbResult<ReportResponse>), 200)]
	[ProducesResponseType(typeof(MbResult<object>), 400)]
	[HttpPost("task/{taskId:long}")]
	public async Task<IActionResult> GenerateTaskReport([FromRoute] long taskId)
	{
		var response = await reportService.GenerateTaskReportAsync(taskId);

		return Ok(MbResultFactory.WithSuccess(response));
	}

	[ProducesResponseType(typeof(MbResult<ReportResponse>), 200)]
	[ProducesResponseType(typeof(MbResult<object>), 400)]
	[HttpPost("group/{groupId:long}")]
	public async Task<IActionResult> GenerateGroupReport([FromRoute] long groupId)
	{
		var response = await reportService.GenerateGroupReportAsync(groupId);

		return Ok(MbResultFactory.WithSuccess(response));
	}
}
