using System.ComponentModel.DataAnnotations;
using ProjectTracker.Infrastructure.Enums;

namespace ProjectTracker.PdfReport.ObjectStorage.Dtos.TaskGroupReport;

public class TaskGroupTaskDto
{
	[Display(Name = "Идентификатор")]
	public long Id { get; set; }

	[Display(Name = "Название")]
	public required string Name { get; set; }

	[Display(Name = "Дедлайн")]
	public DateTime? Deadline { get; set; }

	[Display(Name = "Дата создания")]
	public DateTime CreatedAt { get; set; }

	[Display(Name = "Приоритет")]
	public TaskPriority Priority { get; set; }

	[Display(Name = "Статус")]
	public required TaskGroupTaskStatusDto Status { get; set; }

	[Display(Name = "Исполнители")]
	public required IEnumerable<TaskGroupEmployeeDto> Performers { get; set; }

}
