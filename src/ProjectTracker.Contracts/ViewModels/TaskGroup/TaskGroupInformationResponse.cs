using ProjectTracker.Contracts.ViewModels.Task;

namespace ProjectTracker.Contracts.ViewModels.TaskGroup;

public class TaskGroupInformationResponse : TaskGroupBaseResponse
{
	public ICollection<TaskWithStatusEmployeesReponse> Tasks { get; set; } = [];
}
