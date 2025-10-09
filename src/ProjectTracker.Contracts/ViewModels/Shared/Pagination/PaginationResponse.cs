namespace ProjectTracker.Contracts.ViewModels.Shared.Pagination;

public class PaginationResponse<T> where T : class
{
	public IEnumerable<T> Values { get; set; } = [];
	public long TotalPages { get; set; }
}
