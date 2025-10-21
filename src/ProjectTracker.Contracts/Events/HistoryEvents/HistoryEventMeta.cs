namespace ProjectTracker.Contracts.Events.HistoryEvents;

public class HistoryEventMeta
{
	public long Id { get; set; }
	public HistoryEventType EventType { get; set; }

	public static HistoryEventMeta CreateMeta(long id, HistoryEventType eventType)
	{
		return new HistoryEventMeta
		{
			Id = id,
			EventType = eventType
		};
	}
}