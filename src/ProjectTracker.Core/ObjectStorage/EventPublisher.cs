using MassTransit;
using MassTransit.Util;
using ProjectTracker.Core.ObjectStorage.Interfaces;

namespace ProjectTracker.Core.ObjectStorage;

public class EventPublisher(ISendEndpointProvider endpointProvider) : IEventPublisher
{
	public async Task Publish(object @event, string routingKey, string exchangeName)
	{
		var uri = new Uri($"exchange:{exchangeName}?type=topic");
		var sendEndpoint = await endpointProvider.GetSendEndpoint(uri);

		await sendEndpoint.Send(@event, x => x.SetRoutingKey(routingKey));
	}
}
