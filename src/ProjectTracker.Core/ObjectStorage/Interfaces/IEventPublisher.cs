namespace ProjectTracker.Core.ObjectStorage.Interfaces;

// TODO посмотреть убрать из-за ненадобности
public interface IEventPublisher
{
	Task Publish(object @event, string routingKey, string exchangeName);
}
