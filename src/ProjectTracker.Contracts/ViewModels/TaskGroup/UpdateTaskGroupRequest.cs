namespace ProjectTracker.Contracts.ViewModels.TaskGroup;

public class UpdateTaskGroupRequest
{
	public long Id { get; set; }
	public required string Name { get; set; }
}