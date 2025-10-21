using ProjectTracker.Core.ObjectStorage.Events.Interfaces;

namespace ProjectTracker.Contracts.Events.HistoryEvents;

public class HistoryEvent : IEvent
{
	public required HistoryEventMeta Meta { get; set; }
	public IEnumerable<PropertyChange> Changes { get; set; } = [];
}
