namespace ProjectTracker.Core.ObjectStorage.Dtos.History.Task;

public class TaskPerformersDto : IHistoryDto
{
	public IEnumerable<long> PerformerIds { get; set; } = [];
}
