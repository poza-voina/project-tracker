using ProjectTracker.Infrastructure.Enums;

namespace ProjectTracker.Infrastructure.Models;

public class TaskModel : IDatabaseModel, IConcurrencyModel
{
	public long Id { get; set; }
	public required string Name { get; set; }
	public string? Description { get; set; }
	public long ProjectId { get; set; }
	public long? GroupId { get; set; }
	public DateTime? Deadline { get; set; }
	public DateTime CreatedAt { get; set; }
	public long TaskFlowNodeId { get; set; }
	public TaskPriority Priority { get; set; }
	public uint Version { get; set; }
	public virtual TaskFlowNodeModel? Status { get; set; }
	public virtual ICollection<EmployeeModel> Performers { get; set; } = [];
	public virtual ICollection<EmployeeModel> Observers { get; set; } = [];
	public virtual TaskGroupModel? TaskGroup { get; set; }
	public virtual ProjectModel? Project { get; set; }
}
