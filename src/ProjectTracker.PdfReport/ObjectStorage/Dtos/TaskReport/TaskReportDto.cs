using System.ComponentModel.DataAnnotations;
using ProjectTracker.Infrastructure.Enums;

namespace ProjectTracker.PdfReport.ObjectStorage.Dtos.TaskReport;

public class TaskReportDto
{
	[Display(Name = "Идентификатор")]
	public long Id { get; set; }

	[Display(Name = "Название")]
	public required string Name { get; set; }

	[Display(Name = "Описание")]
	public string? Description { get; set; }

	[Display(Name = "Дедлайн")]
	public DateTime? Deadline { get; set; }

	[Display(Name = "Дата создания")]
	public DateTime CreatedAt { get; set; }

	[Display(Name = "Приоритет")]
	public TaskPriority Priority { get; set; }

	[Display(Name = "Версия сущности на момент создания отчета")]
	public uint Version { get; set; }

	public required TaskReportProjectDto Project { get; set; }
	public TaskReportGroupDto? Group { get; set; }
	public required IEnumerable<TaskReportEmployeeDto> Observers { get; set; }
	public required IEnumerable<TaskReportEmployeeDto> Performers { get; set; }
}