namespace ProjectTracker.Contracts.Events.PublishEvents.Task;

public class EmployeeEventBase : TaskEventBase
{
	public long EmployeeId { get; set; }
}