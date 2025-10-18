using Microsoft.EntityFrameworkCore;
using ProjectTracker.Abstractions.Exceptions;
using ProjectTracker.Infrastructure.DatabaseConstants;
using ProjectTracker.Infrastructure.Models;
using ProjectTracker.Infrastructure.Repositories.Interfaces;
using System.ComponentModel;

namespace ProjectTracker.Infrastructure.Repositories;

public class Repository<TModel>(ApplicationDbContext dbContext) : IRepository<TModel> where TModel : class, IDatabaseModel
{
	public async Task<TModel> AddAsync(TModel entity, CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(entity);

		try
		{
			await dbContext.AddAsync(entity, cancellationToken);
			await dbContext.SaveChangesAsync(cancellationToken);
			dbContext.Entry(entity).State = EntityState.Detached;
		}
		catch (DbUpdateException ex)
		{
			ProcessDatabaseException(ex.InnerException);

			throw;
		}

		return entity;
	}

	private void ProcessDatabaseException(Exception? ex)
	{
		if (ex is Npgsql.PostgresException postgresException)
		{
			var newException = postgresException.SqlState switch
			{
				PostgresStates.FkError => throw new BadRequestException("Несуществующий внешний ключ"),
				PostgresStates.UniqueError => throw new BadRequestException("Сущность уже существует"),
				_ => new InvalidEnumArgumentException($"Нет обработчика для sqlState = {postgresException.SqlState}")
			};
		}
	}

	public async Task AddRangeAsync(
		IEnumerable<TModel> entities,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(entities);

		await dbContext.AddRangeAsync(entities, cancellationToken);
		await dbContext.SaveChangesAsync(cancellationToken);

		foreach (TModel entity in entities)
		{
			dbContext.Entry(entity).State = EntityState.Detached;
		}
	}

	public async Task AddRangeAsync(params TModel[] entities) =>
		await AddRangeAsync(entities.AsEnumerable());

	public Task<bool> CanConnectToDbAsync(CancellationToken cancellationToken = default) =>
		dbContext.Database.CanConnectAsync(cancellationToken);

	public async Task DeleteAsync(
		long id,
		CancellationToken cancellationToken = default)
	{
		var entity = await FindAsync(id);

		await DeleteAsync(entity, cancellationToken);
	}

	public async Task DeleteAsync(
		TModel entity,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(entity);

		dbContext.Remove(entity);
		await dbContext.SaveChangesAsync(cancellationToken);
		dbContext.Entry(entity).State = EntityState.Detached;
	}

	public async Task DeleteRangeAsync(params TModel[] entities)
	{
		if (entities.Length > 0)
		{
			foreach (var entity in entities)
			{
				dbContext.Entry(entity).State = EntityState.Deleted;
			}

			dbContext.RemoveRange(entities);
			await dbContext.SaveChangesAsync();
		}
	}

	public async Task<int> ExecuteSqlAsync(
		FormattableString sqlQuery,
		CancellationToken cancellationToken = default) =>
		await dbContext.Database.ExecuteSqlAsync(sqlQuery, cancellationToken);

	public async Task<TModel> FindAsync(params object[] objects)
	{
		ArgumentNullException.ThrowIfNull(objects);
		var entity = await dbContext.FindAsync<TModel>(objects)
			?? throw new NotFoundException(string.Format("сущность с идентификаторами = [{0}] не найдены", string.Join(',', objects)));
		dbContext.Entry(entity).State = EntityState.Detached;

		return entity;
	}

	public IQueryable<TModel> GetAll() =>
		dbContext
			.Set<TModel>();

	public async Task<TModel> UpdateAsync(
		TModel entity,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(entity);

		try
		{
			if (dbContext.Entry(entity).State is EntityState.Detached)
			{
				dbContext.Entry(entity).State = EntityState.Modified;
			}

			dbContext.Update(entity);
			await dbContext.SaveChangesAsync(cancellationToken);
			dbContext.Entry(entity).State = EntityState.Detached;
		}
		catch (DbUpdateException ex)
		{
			ProcessDatabaseException(ex.InnerException);
		}

		return entity;
	}

	public async Task UpdateRangeAsync(params TModel[] entities)
	{
		foreach (TModel entity in entities)
		{
			await UpdateAsync(entity);
		}
	}
}