using MassTransit;
using ProjectTracker.Abstractions.ConfigurationObjects;
using ProjectTracker.Contracts.Events.PublishEvents.Shared;
using ProjectTracker.Core.ObjectStorage.Events.Interfaces;
using ProjectTracker.Core.ObjectStorage.Interfaces;

namespace ProjectTracker.Core.ObjectStorage;

public class EventDispatcher(
	RabbitMqConfiguration rabbitMqConfiguration,
	IEventCollector eventCollector,
	IPublishEndpoint endpoint) : IEventDispatcher
{
	public async Task DispatchAllAsync()
	{
		var historyEvents = eventCollector
			.GetEvents()
			.Where(x => x is ITaskEvent)
			.Select(EventWrapper.Wrap);

		foreach (var item in historyEvents)
		{
			await endpoint.Publish(
				item,
				x => x.SetRoutingKey(rabbitMqConfiguration.HistoryEndpoint.RoutingKey));
		}
	}
}