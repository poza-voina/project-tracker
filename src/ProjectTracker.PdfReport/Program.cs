using ProjectTracker.PdfReport;

var builder = Host.CreateApplicationBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddMasstransitConfiguration(configuration);
services.AddServices();

var host = builder.Build();
host.Run();
