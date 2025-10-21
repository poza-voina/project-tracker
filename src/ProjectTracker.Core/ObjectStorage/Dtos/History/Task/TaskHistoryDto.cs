using ProjectTracker.Infrastructure.Enums;

namespace ProjectTracker.Core.ObjectStorage.Dtos.History.Task;

public class TaskHistoryDto : IHistoryDto
{
	public required string Name { get; set; }
	public string? Description { get; set; }
	public long ProjectId { get; set; }
	public long? GroupId { get; set; }
	public DateTime? Deadline { get; set; }
	public DateTime CreatedAt { get; set; }
	public long TaskFlowNodeId { get; set; }
	public TaskPriority Priority { get; set; }
}
