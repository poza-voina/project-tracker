using ProjectTracker.Contracts.ViewModels.TaskFlowNode;

namespace ProjectTracker.Contracts.ViewModels.Task;

public class TaskWithStatusResponse : TaskBaseReponse
{
	public required TaskFlowNodeResponse Status { get; set; }
}