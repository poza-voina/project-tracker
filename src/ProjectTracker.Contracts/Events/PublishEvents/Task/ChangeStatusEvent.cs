namespace ProjectTracker.Contracts.Events.PublishEvents.Task;

using ProjectTracker.Core.ObjectStorage.Events.Interfaces;

public class ChangeStatusEvent : TaskEventBase
{
	public required string Status { get; set; }
}
