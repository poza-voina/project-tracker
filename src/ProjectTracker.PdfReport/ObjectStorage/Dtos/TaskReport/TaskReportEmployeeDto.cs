using System.ComponentModel.DataAnnotations;
using ProjectTracker.Infrastructure.Enums;

namespace ProjectTracker.PdfReport.ObjectStorage.Dtos.TaskReport;

public class TaskReportEmployeeDto
{
	[Display(Name = "Идентификатор")]
	public long Id { get; set; }

	[Display(Name = "Фамилия")]
	public required string LastName { get; set; }

	[Display(Name = "Имя")]
	public required string FirstName { get; set; }

	[Display(Name = "Отчество")]
	public string? Patronymic { get; set; }

	[Display(Name = "Имя пользователя")]
	public required string Username { get; set; }

	[Display(Name = "Роль")]
	public EmployeeRole Role { get; set; }
}


