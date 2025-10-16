namespace ProjectTracker.Core.ObjectStorage.Interfaces;

public interface IEventPublisher
{
	Task Publish(object @event, string routingKey, string exchangeName);
}
