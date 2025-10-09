using Microsoft.EntityFrameworkCore;

namespace ProjectTracker.Core.Extensions;

public static class CollectionExtensions
{
	public static IQueryable<T> Paginate<T>(this IQueryable<T> source, int pageNumber, int pageSize)
	{
		return source.Skip((pageNumber - 1) * pageSize).Take(pageSize);
	}

	public static async Task<long> GetTotalPagesAsync<T>(this IQueryable<T> source, int pageSize)
	{
		var totalCount = await source.CountAsync();

		return (long)Math.Ceiling(totalCount / (double)pageSize);
	}
}