namespace ProjectTracker.History.Infrastructure.Models;

public class TaskHistoryMetaModel : IDatabaseModel
{
	public long Id { get; set; }
	public long TaskId { get; set; }
	public virtual ICollection<TaskHistoryRecordModel> TaskHistoryRecords { get; set; } = [];
}
