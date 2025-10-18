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
	[HttpPost("task")]
	public async Task<IActionResult> GenerateTaskReport([FromBody] TaskReportRequest request)
	{
		var response = await reportService.GenerateTaskReportAsync(request);

		return Ok(MbResultFactory.WithSuccess(response));
	}

	[ProducesResponseType(typeof(MbResult<ReportResponse>), 200)]
	[ProducesResponseType(typeof(MbResult<object>), 400)]
	[HttpPost("group")]
	public async Task<IActionResult> GenerateGroupReport([FromBody] TaskGroupReportRequest request)
	{
		var response = await reportService.GenerateGroupReportAsync(request);

		return Ok(MbResultFactory.WithSuccess(response));
	}
}
