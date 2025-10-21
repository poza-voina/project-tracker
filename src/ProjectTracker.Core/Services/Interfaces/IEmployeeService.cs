using ProjectTracker.Contracts.ViewModels.Employee;
using ProjectTracker.Contracts.ViewModels.Shared.Pagination;

namespace ProjectTracker.Core.Services.Interfaces;

public interface IEmployeeService
{
	Task<EmployeeBaseResponse> CreateAsync(CreateEmployeeRequest request);
	Task DeleteAsync(DeleteEmployeeRequest request);
	Task<PaginationResponse<EmployeeBaseResponse>> GetAllAsync(PaginationRequest request);
	Task<EmployeeBaseResponse> GetAsync(GetEmployeeRequest request);
}
