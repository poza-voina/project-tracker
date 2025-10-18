namespace ProjectTracker.Contracts.Events.Reports;

public class ReportErrorEvent
{
	public required Guid ReportId { get; init; }
	public required string Message { get; init; }
}
