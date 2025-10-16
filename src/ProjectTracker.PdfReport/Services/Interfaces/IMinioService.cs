namespace ProjectTracker.PdfReport.Services.Interfaces;

public interface IMinioService
{
	Task<string> UploadFileAndGetUrlAsync(string bucketName, string objectName, Stream fileStream, string contentType);
	Task<string> GetFileUrlAsync(string bucketName, string objectName);
}
