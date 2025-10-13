namespace ProjectTracker.Contracts.ViewModels.TaskFlowNode;

public class TaskFlowNodeResponse
{
	public long Id { get; set; }
	public required string Name { get; set; }
	public long TaskFlowId { get; set; }
}
