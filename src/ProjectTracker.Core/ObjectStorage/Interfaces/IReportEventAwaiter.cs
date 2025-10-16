using ProjectTracker.Contracts.Events.PublishEvents.Shared;
using ProjectTracker.Contracts.Events.Reports;
using ProjectTracker.Core.ObjectStorage.Events.Interfaces;

namespace ProjectTracker.Core.ObjectStorage.Interfaces;

public interface IReportEventAwaiter
{
	ReportEventAwaiter AddReportId(Guid reportId);
	Task<string> WaitEvent(Guid reportId, TimeSpan? timeout = null);
	void ProcessResultEvent(ReportResultEvent @event);
}


