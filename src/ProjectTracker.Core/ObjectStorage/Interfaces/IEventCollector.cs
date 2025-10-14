using ProjectTracker.Core.ObjectStorage.Events.Interfaces;
using System.Collections.Generic;

namespace ProjectTracker.Core.ObjectStorage.Interfaces;

public interface IEventCollector
{
	EventCollector Add(IEvent @event);
	IEnumerable<IEvent> GetEvents();
	void Clear();
}
