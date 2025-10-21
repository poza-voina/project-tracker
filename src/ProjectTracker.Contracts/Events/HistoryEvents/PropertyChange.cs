namespace ProjectTracker.Contracts.Events.HistoryEvents;

public class PropertyChange
{
	public required string Property { get; set; }
	public string? OldValue { get; set; }
	public string? NewValue { get; set; }
}