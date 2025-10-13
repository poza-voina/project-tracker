namespace ProjectTracker.Infrastructure.Models;

public class TaskFlowModel : IDatabaseModel
{
	public long Id { get; set; }
	public long? ProjectDeletableStatusId { get; set; }
	public virtual TaskFlowNodeModel? ProjectDeletableStatus { get; set; }
	public virtual ICollection<ProjectModel> Projects { get; set; } = [];
	public virtual ICollection<TaskFlowEdgeModel> Edges { get; set; } = [];
	public virtual ICollection<TaskFlowNodeModel> Nodes { get; set; } = [];
}
