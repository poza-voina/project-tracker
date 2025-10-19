namespace ProjectTracker.Contracts.ViewModels.Report;

public class ReportRequestBase
{
	public int ExpirySeconds { get; set; }
	public TimeSpan? Timeout { get; set; }
}