using ProjectTracker.PdfReport;
using QuestPDF.Infrastructure;

var builder = Host.CreateApplicationBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

QuestPDF.Settings.License = LicenseType.Community;

services.AddHttpClient();
services.AddSignletonConfigurations(configuration);
services.AddMasstransitConfiguration(configuration);
services.AddServices();
services.AddMinioConfiguration(configuration);

var host = builder.Build();
host.Run();
