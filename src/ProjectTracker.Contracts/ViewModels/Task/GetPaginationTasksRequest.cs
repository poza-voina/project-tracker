using ProjectTracker.Contracts.ViewModels.Shared.Pagination;

namespace ProjectTracker.Contracts.ViewModels.Task;

public class GetPaginationTasksRequest : PaginationRequest
{
	public long? ProjectId { get; set; }
	public long? GroupId { get; set; }
	public long? EmployeeId { get; set; }
}
