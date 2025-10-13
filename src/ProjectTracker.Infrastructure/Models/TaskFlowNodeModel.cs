using ProjectTracker.Infrastructure.Enums;

namespace ProjectTracker.Infrastructure.Models;

public class TaskFlowNodeModel : IDatabaseModel
{
	public long Id { get; set; }
	public required string Name { get; set; }
	public long TaskFlowId { get; set; }
	public TaskFlowNodeStatus? Status { get; set; }
	public virtual TaskFlowModel? TaskFlow{ get; set; }
	public virtual ICollection<TaskModel> TaskModels { get; set; } = [];
	public virtual ICollection<TaskFlowEdgeModel> FromEdges { get; set; } = [];
	public virtual ICollection<TaskFlowEdgeModel> ToEdges { get; set; } = [];
}