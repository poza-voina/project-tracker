using ProjectTracker.Contracts.Events.Reports;
using ProjectTracker.Contracts.ViewModels.Report;

namespace ProjectTracker.Core.Services.Interfaces;

public interface IReportService
{
	Task<ReportResponse> GenerateTaskReportAsync(TaskReportRequest request);
	Task<ReportResponse> GenerateGroupReportAsync(TaskGroupReportRequest request);
	Task ProcessReportResultAsync(ReportResultEvent resultEvent);
	Task ProcessReportErrorAsync(Guid reportId);
	Task<ReportResponse> GetReportAsync(Guid reportId);
}
