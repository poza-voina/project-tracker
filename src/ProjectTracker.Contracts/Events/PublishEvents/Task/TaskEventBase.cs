namespace ProjectTracker.Contracts.Events.PublishEvents.Task;

using ProjectTracker.Core.ObjectStorage.Events.Interfaces;

public abstract class TaskEventBase : ITaskEvent
{
	public long TaskId { get; set; }
	public long ProjectId { get; set; }
	public long? GroupId { get; set; }
}