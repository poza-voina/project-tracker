using System.ComponentModel.DataAnnotations;

namespace ProjectTracker.PdfReport.ObjectStorage.Dtos.TaskReport;

public class TaskReportGroupDto
{
	[Display(Name = "Идентификатор группы")]
	public long Id { get; set; }

	[Display(Name = "Название группы")]
	public required string Name { get; set; }
}


