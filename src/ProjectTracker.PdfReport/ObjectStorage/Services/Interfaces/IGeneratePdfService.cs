using ProjectTracker.Contracts.Events.Reports;

namespace ProjectTracker.PdfReport.ObjectStorage.Services.Interfaces;

public interface IGeneratePdfService
{
	Task<string> GenerateTaskReportAsync(ReportInputTaskEvent inputEvent);
	Task<string> GenerateTaskGroupReportAsync(ReportInputTaskGroupEvent inputEvent);
}
