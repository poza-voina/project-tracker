using ProjectTracker.Contracts.Events.Interfaces;

namespace ProjectTracker.Contracts.Events.Reports;

public abstract class ReportInputEventBase : IReportEvent
{
	public Guid ReportId { get; set; }
	public int ExpirySeconds { get;set; }
}