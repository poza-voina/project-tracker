using ProjectTracker.Contracts.Events.Reports;

namespace ProjectTracker.PdfReport.Services.Interfaces;

public interface IGeneratePdfService
{
	Task<string> GenerateTaskReport(ReportInputEventBase inputEvent);
}


