using ProjectTracker.Contracts.ViewModels.Employee;
using ProjectTracker.Contracts.ViewModels.TaskFlowNode;

namespace ProjectTracker.Contracts.ViewModels.Task;

public class TaskWithStatusEmployeesReponse : TaskBaseReponse
{
	public required TaskFlowNodeResponse Status { get; set; }
	public ICollection<EmployeeBaseResponse> Performers { get; set; } = [];
	public ICollection<EmployeeBaseResponse> Observers { get; set; } = [];
}