namespace ProjectTracker.History.Infrastructure.Models;

public class TaskHistoryRecordModel : IDatabaseModel
{
	public long Id { get; set; }
	public long MetaId { get; set; }
	public required string ChangeEventType { get; set; }
	public required string Property { get; set; }
	public string? OldValue { get; set; }
	public string? NewValue { get; set; }
	public DateTime CreatedAt { get; set; }
	public virtual TaskHistoryMetaModel? Meta { get; set; }
}