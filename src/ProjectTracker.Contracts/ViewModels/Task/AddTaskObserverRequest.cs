namespace ProjectTracker.Contracts.ViewModels.Task;

public class AddTaskObserverRequest
{
	public long EmployeeId { get; set; }
	public long TaskId { get; set; }
}