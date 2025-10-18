using System.ComponentModel.DataAnnotations;

namespace ProjectTracker.PdfReport.ObjectStorage.Dtos.TaskReport;

public class TaskReportProjectDto
{
	[Display(Name = "Идентификатор")]
	public long Id { get; set; }

	[Display(Name = "Название проекта")]
	public required string Name { get; set; }

	public required TaskReportEmployeeDto ProjectManager { get; set; }
	public TaskReportEmployeeDto? Manager { get; set; }
}


