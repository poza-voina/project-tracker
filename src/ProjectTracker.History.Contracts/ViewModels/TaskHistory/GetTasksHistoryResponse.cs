namespace ProjectTracker.History.Contracts.ViewModels.TaskHistory;

public class GetTasksHistoryResponse
{
	public required string ChangeEventType { get; set; }
	public required string Property { get; set; }
	public string? OldValue { get; set; }
	public string? NewValue { get; set; }
	public DateTime CreatedAt { get; set; }
}
