using ProjectTracker.Contracts.Events.Reports;
using ProjectTracker.Contracts.ViewModels.Report;

namespace ProjectTracker.Core.Services.Interfaces;

public interface IReportService
{
	Task<ReportResponse> GenerateTaskReportAsync(long taskId);
	Task<ReportResponse> GenerateGroupReportAsync(long groupId);
}
