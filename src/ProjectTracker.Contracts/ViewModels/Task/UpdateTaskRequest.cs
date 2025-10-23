using ProjectTracker.Infrastructure.Enums;

namespace ProjectTracker.Contracts.ViewModels.Task;

public class UpdateTaskRequest
{
	public long Id { get; set; }
	public required string Name { get; set; }
	public string? Description { get; set; }
	public DateTime? Deadline { get; set; }
	public TaskPriority Priority { get; set; }
}
