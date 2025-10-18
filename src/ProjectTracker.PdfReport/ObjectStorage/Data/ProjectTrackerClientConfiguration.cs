namespace ProjectTracker.PdfReport.ObjectStorage.Data;

public class ProjectTrackerClientConfiguration
{
	public required string TasksBasePath { get; set; }
	public required string TaskGroupsBasePath { get; set; }
	public required string ClientName { get; set; }
	public required string DefaultUri { get; set; }
}
