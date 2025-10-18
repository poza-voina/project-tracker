using System.ComponentModel.DataAnnotations;

namespace ProjectTracker.PdfReport.ObjectStorage.Dtos.TaskGroupReport;

public class TaskGroupReportDto
{
	[Display(Name = "Идентификатор")]
	public long Id { get; set; }

	[Display(Name = "Название")]
	public required string Name { get; set; }

	[Display(Name = "Задачи")]
	public required IEnumerable<TaskGroupTaskDto> Tasks { get; set; }
}
