using Microsoft.EntityFrameworkCore;
using System.Reflection;
using ProjectTracker.History.Infrastructure.Models;

namespace ProjectTracker.History.Infrastructure;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
	public DbSet<TaskHistoryRecordModel> TaskHistoryRecords => Set<TaskHistoryRecordModel>();
	public DbSet<TaskHistoryMetaModel> TaskHistoryMetas => Set<TaskHistoryMetaModel>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
	}
}
