using ProjectTracker.Contracts.Events.Reports;

namespace ProjectTracker.Core.ObjectStorage.Interfaces;

public interface IReportEventAwaiter
{
	ReportEventAwaiter AddReportId(Guid reportId);
	Task<string?> WaitEvent(Guid reportId, TimeSpan? timeout = null);
	bool IsExists(Guid reportId);
	void ProcessResultEvent(ReportResultEvent @event);
	void ProcessErrorEvent(ReportErrorEvent @event);
}