namespace ProjectTracker.Core.ObjectStorage.Dtos.History.Task;

public class TaskStatusHistoryDto : IHistoryDto
{
	public required long TaskFlowNodeId { get; set; }
}
