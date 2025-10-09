namespace ProjectTracker.Infrastructure.Models;

public class TaskGroupModel : IDatabaseModel<long>
{
	public long Id { get; set; }
	public required string Name { get; set; }
	public virtual ICollection<TaskModel>? Tasks { get; set; }
}