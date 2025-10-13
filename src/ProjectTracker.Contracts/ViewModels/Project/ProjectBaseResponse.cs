namespace ProjectTracker.Contracts.ViewModels.Project;

public class ProjectBaseResponse
{
	public long Id { get; set; }
	public required string Name { get; set; }
	public string? Description { get; set; }
	public long ProjectManagerId { get; set; }
	public long? ManagerId { get; set; }
	public long TaskFlowId { get; set; }
}