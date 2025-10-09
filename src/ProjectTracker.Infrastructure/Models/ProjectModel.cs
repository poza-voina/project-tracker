namespace ProjectTracker.Infrastructure.Models;

public class ProjectModel : IDatabaseModel<long>
{
	public long Id { get; set; }
	public required string Name { get; set; }
	public string? Description { get; set; }
	public long ProjectManagerId { get; set; }
	public long? ManagerId { get; set; }
	public virtual EmployeeModel? ProjectManager { get; set; }
	public virtual EmployeeModel? Manager { get; set; }
	public virtual ICollection<TaskModel>? Tasks { get; set; }
}