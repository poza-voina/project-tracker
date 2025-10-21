namespace ProjectTracker.Core.ObjectStorage.Dtos.History.Task;

public class TaskObserversDto : IHistoryDto
{
	public IEnumerable<long> ObserverIds { get; set; } = [];
}
