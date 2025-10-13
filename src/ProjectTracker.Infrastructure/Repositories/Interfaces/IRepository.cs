using ProjectTracker.Infrastructure.Models;

namespace ProjectTracker.Infrastructure.Repositories.Interfaces;

public interface IRepository<TModel> where TModel : class, IDatabaseModel
{
	Task<TModel> AddAsync(
		TModel entity,
		CancellationToken cancellationToken = default);

	Task AddRangeAsync(
		IEnumerable<TModel> entities,
		CancellationToken cancellationToken = default);

	Task AddRangeAsync(params TModel[] entities);

	Task<bool> CanConnectToDbAsync(CancellationToken cancellationToken = default);

	Task DeleteAsync(
		long id,
		CancellationToken cancellationToken = default);

	Task DeleteAsync(
		TModel entity,
		CancellationToken cancellationToken = default);

	Task DeleteRangeAsync(params TModel[] entities);

	Task<TModel> FindAsync(params object[] objects);

	IQueryable<TModel> GetAll();

	Task<TModel> UpdateAsync(
		TModel entity,
		CancellationToken cancellationToken = default);

	Task UpdateRangeAsync(params TModel[] entities);

	Task<int> ExecuteSqlAsync(
		FormattableString sqlQuery,
		CancellationToken cancellationToken = default);
}