namespace ProjectTracker.Infrastructure.Models;

public class TaskFlowEdgeModel : IDatabaseModel
{
	public long Id { get; set; }
	public long? FromNodeId { get; set; }
	public long? ToNodeId { get; set; }
	public long? TaskFlowId { get; set; }
	public virtual TaskFlowModel? TaskFlow { get; set; }
	public virtual TaskFlowNodeModel? FromNode { get; set; }
	public virtual TaskFlowNodeModel? ToNode { get; set; }
}