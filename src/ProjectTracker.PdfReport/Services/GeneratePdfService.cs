using MassTransit;
using ProjectTracker.Contracts.Events.Reports;
using ProjectTracker.PdfReport.Services.Interfaces;

namespace ProjectTracker.PdfReport.Services;

public class GeneratePdfService(
	IClientFactory clientFactory,
	IMinioService minioService) : IGeneratePdfService
{
	public async Task<string> GenerateTaskReport(ReportInputEventBase inputEvent)
	{
		throw new NotImplementedException(); //TODO сделать
	}
}


