using ProjectTracker.Core.ObjectStorage.Events.Interfaces;

namespace ProjectTracker.Contracts.Events.Reports;

public class ReportResultEvent : IEvent
{
	public Guid ReportId { get; set; }
	public required string Url { get;set;}
}