using ProjectTracker.Contracts.ViewModels.Employee;
using ProjectTracker.Core.Services.Interfaces;
using ProjectTracker.Infrastructure.Models;
using ProjectTracker.Infrastructure.Repositories.Interfaces;
using Mapster;
using ProjectTracker.Contracts.ViewModels.Shared.Pagination;
using ProjectTracker.Core.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ProjectTracker.Core.Services;

public class EmployeeService(IRepository<EmployeeModel> employeeRepository) : IEmployeeService
{
	public async Task<EmployeeBaseResponse> CreateAsync(CreateEmployeeRequest request)
	{
		var model = request.Adapt<EmployeeModel>();

		var result = await employeeRepository.AddAsync(model);

		return result.Adapt<EmployeeBaseResponse>();
	}

	public async Task DeleteAsync(DeleteEmployeeRequest request)
	{
		await employeeRepository.DeleteAsync(request.Id);
	}

	public async Task<PaginationResponse<EmployeeBaseResponse>> GetAllAsync(PaginationRequest request)
	{
		var query = employeeRepository
			.GetAll();

		var totalPages = await query.GetTotalPagesAsync(request.PageSize);

		var items = await query
			.Paginate(request.PageNumber, request.PageSize)
			.ToListAsync();

		return new PaginationResponse<EmployeeBaseResponse>
		{
			Values = items.Adapt<IEnumerable<EmployeeBaseResponse>>(),
			TotalPages = totalPages
		};
	}

	public async Task<EmployeeBaseResponse> GetAsync(GetEmployeeRequest request)
	{
		var model = await employeeRepository.FindAsync(request.Id);

		return model.Adapt<EmployeeBaseResponse>();
	}
}
