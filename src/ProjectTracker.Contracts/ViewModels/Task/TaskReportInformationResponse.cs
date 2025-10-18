using ProjectTracker.Contracts.ViewModels.Employee;
using ProjectTracker.Contracts.ViewModels.Project;
using ProjectTracker.Contracts.ViewModels.TaskGroup;

namespace ProjectTracker.Contracts.ViewModels.Task;

public class TaskReportInformationResponse : TaskBaseReponse
{
	public required IEnumerable<EmployeeBaseResponse> Observers { get; set; }
	public required IEnumerable<EmployeeBaseResponse> Performers { get; set; }
	public required ProjectWithManagersResponse Project { get; set; }
	public TaskGroupBaseResponse? Group { get; set; }
}
