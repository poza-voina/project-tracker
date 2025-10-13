namespace ProjectTracker.Contracts.ViewModels.Task;

public class ChangeTaskStatusRequest
{
	public long TaskId { get; set; }
	public long TaskFlowNodeId { get; set; }
	public uint TaskVersion { get; set; }
}
