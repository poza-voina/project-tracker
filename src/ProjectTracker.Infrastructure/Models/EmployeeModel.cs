using ProjectTracker.Infrastructure.Enums;

namespace ProjectTracker.Infrastructure.Models;

public class EmployeeModel : IDatabaseModel
{
	public long Id { get; set; }
	public required string LastName { get; set; }
	public required string FirstName { get; set; }
	public string? Patronymic { get; set; }
	public required string Username { get; set; }
	public EmployeeRole Role { get; set; }
	public virtual ICollection<ProjectModel>? ManagedProjects { get; set; }
	public virtual ICollection<ProjectModel>? SupervisedProjects { get; set; }
	public virtual ICollection<TaskModel>? PerformedTasks { get; set; }
	public virtual ICollection<TaskModel>? ObservedTasks { get; set; }
}
