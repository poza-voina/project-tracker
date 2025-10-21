using MassTransit;
using ProjectTracker.Contracts.Events.HistoryEvents;
using ProjectTracker.Core.ObjectStorage.Interfaces;

namespace ProjectTracker.Core.ObjectStorage;

public class EventDispatcher(
	IEventCollector eventCollector,
	IPublishEndpoint endpoint) : IEventDispatcher
{
	public async Task DispatchAllAsync()
	{
		var historyEvents = eventCollector
			.GetEvents()
			.Where(x => x is HistoryEvent);

		foreach (var item in historyEvents)
		{
			await endpoint.Publish<HistoryEvent>(item);
		}
	}
}