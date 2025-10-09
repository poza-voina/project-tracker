using ProjectTracker.Infrastructure.Enums;

namespace ProjectTracker.Contracts.ViewModels.Employee;

public class EmployeeBaseResponse
{
	public long Id { get; set; }
	public required string LastName { get; set; }
	public required string FirstName { get; set; }
	public string? Patronymic { get; set; }
	public required string Username { get; set; }
	public EmployeeRole Role { get; set; }
}