using ProjectTracker.Infrastructure.Enums;

namespace ProjectTracker.Infrastructure.Models;

public class ReportModel : IDatabaseModel
{
	public Guid Id { get; set; }
	public string? Url { get; set; }
	public ReportType Type { get; set; }
	public DateTime CreatedAt { get; set; }
	public ReportStatus Status { get; set; }
}
