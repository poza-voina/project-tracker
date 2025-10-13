using ProjectTracker.Contracts.ViewModels.Employee;
using ProjectTracker.Infrastructure.Models;

namespace ProjectTracker.Contracts.ViewModels.Task;

public class TaskWithStatusEmployeesReponse : TaskBaseReponse
{
	public virtual ICollection<EmployeeBaseResponse>? Performers { get; set; }
	public virtual ICollection<EmployeeBaseResponse>? Observers { get; set; }
}