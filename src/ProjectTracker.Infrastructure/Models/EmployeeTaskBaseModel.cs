namespace ProjectTracker.Infrastructure.Models;

public abstract class EmployeeTaskBaseModel : IDatabaseModel
{
	public long TaskId { get; set; }
	public virtual TaskModel? Task { get; set; }

	public long EmployeeId { get; set; }
	public virtual EmployeeModel? Employee { get; set; }
}
