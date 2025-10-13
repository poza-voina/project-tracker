using ProjectTracker.Infrastructure.Enums;

namespace ProjectTracker.Contracts.ViewModels.Task;

public class TaskBaseReponse
{
	public long Id { get; set; }
	public required string Name { get; set; }
	public string? Description { get; set; }
	public long ProjectId { get; set; }
	public long? GroupId { get; set; }
	public DateTime? Deadline { get; set; }
	public DateTime CreatedAt { get; set; }
	public TaskPriority Priority { get; set; }
	public uint Version { get; set; }
}