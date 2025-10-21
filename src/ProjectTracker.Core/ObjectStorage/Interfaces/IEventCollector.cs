using ProjectTracker.Core.ObjectStorage.Events.Interfaces;

namespace ProjectTracker.Core.ObjectStorage.Interfaces;

public interface IEventCollector
{
	EventCollector Add(IEvent? @event);
	IEnumerable<IEvent> GetEvents();
	void Clear();
}
