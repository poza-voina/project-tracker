using ProjectTracker.Core.ObjectStorage.Events.Interfaces;

namespace ProjectTracker.Contracts.Events.Reports;

public abstract class ReportInputEventBase : IEvent
{
	public Guid ReportId { get; set; }
}