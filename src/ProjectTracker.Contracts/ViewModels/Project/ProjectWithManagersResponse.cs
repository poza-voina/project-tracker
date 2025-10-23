using ProjectTracker.Contracts.ViewModels.Employee;

namespace ProjectTracker.Contracts.ViewModels.Project;

public class ProjectWithManagersResponse : ProjectBaseResponse
{
	public EmployeeBaseResponse? ProjectManager { get; set; }
	public EmployeeBaseResponse? Manager { get; set; }
}