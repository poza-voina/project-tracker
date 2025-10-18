using MassTransit;
using ProjectTracker.Core.ObjectStorage.Events.Interfaces;
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
			.Where(x => x is IHistoryTaskEvent);

		foreach (var item in historyEvents)
		{
			await endpoint.Publish<IHistoryTaskEvent>(item);
		}
	}
}