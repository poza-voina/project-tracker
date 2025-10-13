using Microsoft.EntityFrameworkCore;
using ProjectTracker.Infrastructure.Models;
using System.Reflection;

namespace ProjectTracker.Infrastructure;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
	public DbSet<EmployeeModel> Employees => Set<EmployeeModel>();
	public DbSet<ProjectModel> Projects => Set<ProjectModel>();
	public DbSet<TaskGroupModel> TaskGroups => Set<TaskGroupModel>();
	public DbSet<TaskModel> Tasks => Set<TaskModel>();
	public DbSet<TaskFlowModel> TaskFlows => Set<TaskFlowModel>();
	public DbSet<TaskFlowNodeModel> TaskFlowNodes => Set<TaskFlowNodeModel>();
	public DbSet<TaskFlowEdgeModel> TaskFlowEdges => Set<TaskFlowEdgeModel>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
	}
}