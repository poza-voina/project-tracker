using ProjectTracker.Contracts.Events.Interfaces;
using ProjectTracker.Core.ObjectStorage.Events.Interfaces;

namespace ProjectTracker.Contracts.Events.PublishEvents.Shared;

public sealed class EventWrapper<TEvent> : IEventWrapper
{
	public Guid EventId { get; init; }
	public required string EventType { get; init; }
	public DateTimeOffset CreatedAt { get; init; }
	public required TEvent Event { get; init; }

	public static EventWrapper<TEvent> Wrap(TEvent @event)
	{
		if (@event == null) throw new ArgumentNullException(nameof(@event));

		var type = @event.GetType().FullName ?? @event.GetType().Name;

		return new EventWrapper<TEvent>
		{
			EventId = Guid.NewGuid(),
			EventType = type,
			CreatedAt = DateTimeOffset.UtcNow,
			Event = @event
		};
	}
}
