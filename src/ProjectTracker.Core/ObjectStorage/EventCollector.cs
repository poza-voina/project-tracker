using ProjectTracker.Core.ObjectStorage.Events.Interfaces;
using ProjectTracker.Core.ObjectStorage.Interfaces;

namespace ProjectTracker.Core.ObjectStorage;

public class EventCollector : IEventCollector
{
	private readonly List<IEvent> _events = new();

	public EventCollector Add(IEvent @event)
	{
		_events.Add(@event);
		return this;
	}

	public IEnumerable<IEvent> GetEvents() =>
		_events;

	public void Clear()
	{
		_events.Clear();
	}
}
