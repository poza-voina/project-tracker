using Microsoft.AspNetCore.Mvc;
using ProjectTracker.Contracts.ViewModels.Employee;
using ProjectTracker.Contracts.ViewModels.Shared.Pagination;
using ProjectTracker.Core.Services.Interfaces;
using EmployeeErrorResponse = ProjectTracker.Contracts.ViewModels.Shared.Result.MbResult<object>;
using EmployeeResponse = ProjectTracker.Contracts.ViewModels.Shared.Result.MbResult<ProjectTracker.Contracts.ViewModels.Employee.EmployeeBaseResponse>;
using EmployeesResponse = ProjectTracker.Contracts.ViewModels.Shared.Result.MbResult<ProjectTracker.Contracts.ViewModels.Shared.Pagination.PaginationResponse<ProjectTracker.Contracts.ViewModels.Employee.EmployeeBaseResponse>>;

namespace ProjectTracker.Api.Controllers;

//TODO [ProducesResponseType()] сделать везде или убрать
[Route("api/employees")]
public class EmployeeController(IEmployeeService employeeService) : ControllerBase
{
	[ProducesResponseType(typeof(EmployeeResponse), 200)]
	[ProducesResponseType(typeof(EmployeeErrorResponse), 404)]
	[HttpGet("{id:long}")]
	public async Task<IActionResult> GetEmployee([FromRoute] long id)
	{
		var result = await employeeService.GetAsync(id);

		return Ok(result);
	}

	[ProducesResponseType(typeof(EmployeesResponse), 200)]
	[HttpGet]
	public async Task<IActionResult> GetAllEmployee([FromQuery] PaginationRequest request)
	{
		var result = await employeeService.GetAllAsync(request);
		
		return Ok(result);
	}

	[ProducesResponseType(typeof(EmployeeResponse), 200)]
	[ProducesResponseType(typeof(EmployeeErrorResponse), 400)]
	[HttpPost]
	public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeRequest request)
	{
		var result = await employeeService.CreateAsync(request);

		return Ok(result);
	}

	[ProducesResponseType(typeof(EmployeeErrorResponse), 404)]
	[HttpDelete("{id:long}")]
	public async Task<IActionResult> DeleteEmployeeAsync([FromRoute] long id)
	{
		await employeeService.DeleteAsync(id);

		return Ok();
	}
}
