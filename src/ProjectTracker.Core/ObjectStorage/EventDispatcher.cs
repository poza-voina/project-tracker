using MassTransit;
using ProjectTracker.Abstractions.ConfigurationObjects;
using ProjectTracker.Contracts.Events.Interfaces;
using ProjectTracker.Contracts.Events.PublishEvents.Shared;
using ProjectTracker.Core.ObjectStorage.Events.Interfaces;
using ProjectTracker.Core.ObjectStorage.Interfaces;

namespace ProjectTracker.Core.ObjectStorage;

public class EventDispatcher(
	ProjectTrackerRabbitMqConfiguration rabbitMqConfiguration,
	IEventCollector eventCollector,
	IEventPublisher publisher) : IEventDispatcher
{
	public async Task DispatchAllAsync()
	{
		var historyEvents = eventCollector
			.GetEvents()
			.Where(x => x is ITaskEvent)
			.Select(EventWrapper<object>.Wrap);

		foreach (var item in historyEvents)
		{
			await publisher.Publish(
				item,
				rabbitMqConfiguration.HistoryEndpoint.RoutingKey,
				rabbitMqConfiguration.DefaultEndpoint.Name);
		}
	}
}