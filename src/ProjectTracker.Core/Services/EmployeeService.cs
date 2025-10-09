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

	public async Task DeleteAsync(long id)
	{
		await employeeRepository.DeleteAsync(id);
	}

	public async Task<PaginationResponse<EmployeeBaseResponse>> GetAllAsync(PaginationRequest request)
	{
		var query = employeeRepository
			.GetAll();

		var totalPages = await query.GetTotalPagesAsync(request.PageSize);

		var items = query
			.Paginate(request.PageNumber, request.PageSize)
			.ToListAsync();

		return new PaginationResponse<EmployeeBaseResponse>
		{
			Values = query.Adapt<IEnumerable<EmployeeBaseResponse>>(),
			TotalPages = totalPages
		};
	}

	public async Task<EmployeeBaseResponse> GetAsync(long id)
	{
		var model = await employeeRepository.FindAsync(id);

		return model.Adapt<EmployeeBaseResponse>();
	}
}
