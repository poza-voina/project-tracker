namespace ProjectTracker.PdfReport.ObjectStorage.Dtos.TaskReport;

public class TaskReportInputDto
{
	public long TaskId { get; set; }
	public Dictionary<string, string?> TaskProperties { get; set; } = new();
	public Dictionary<string, string?> ProjectManagerProperties { get; set; } = new();
	public Dictionary<string, string?> ManagerProperties { get; set; } = new();
	public Dictionary<string, List<string?>> Observers { get; set; } = new();
	public Dictionary<string, List<string?>> Performers { get; set; } = new();
}
