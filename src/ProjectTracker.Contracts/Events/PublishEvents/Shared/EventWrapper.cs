namespace ProjectTracker.Contracts.Events.PublishEvents.Shared;

public sealed class EventWrapper
{
	public Guid EventId { get; init; }
	public required string EventType { get; init; }
	public DateTimeOffset CreatedAt { get; init; }
	public required object Event { get; init; }

	public static EventWrapper Wrap(object @event)
	{
		if (@event == null) throw new ArgumentNullException(nameof(@event));

		var type = @event.GetType().FullName ?? @event.GetType().Name;

		return new EventWrapper
		{
			EventId = Guid.NewGuid(),
			EventType = type,
			CreatedAt = DateTimeOffset.UtcNow,
			Event = @event
		};
	}
}
