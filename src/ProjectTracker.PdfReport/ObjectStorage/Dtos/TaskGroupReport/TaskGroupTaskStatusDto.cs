using System.ComponentModel.DataAnnotations;
using ProjectTracker.Infrastructure.Enums;

namespace ProjectTracker.PdfReport.ObjectStorage.Dtos.TaskGroupReport;

public class TaskGroupTaskStatusDto
{
	[Display(Name = "Название")]
	public required string Name { get; set; }

	[Display(Name = "Статус")]
	public TaskFlowNodeStatus Status { get; set; }
}
