namespace ProjectTracker.Contracts.Events.Reports;

public abstract class ReportResultEventBase
{
	public Guid ReportId { get; set; }
	public long Url { get; set; }
}