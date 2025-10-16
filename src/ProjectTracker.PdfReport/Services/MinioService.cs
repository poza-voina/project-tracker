using ProjectTracker.PdfReport.Services.Interfaces;

namespace ProjectTracker.PdfReport.Services;

public class MinioService : IMinioService
{
	public async Task<string> UploadFileAndGetUrlAsync(string bucketName, string objectName, Stream fileStream, string contentType)
	{
		//TODO: Implement MinIO file upload and URL generation
		throw new NotImplementedException();
	}

	public async Task<string> GetFileUrlAsync(string bucketName, string objectName)
	{
		//TODO: Implement MinIO URL generation
		throw new NotImplementedException();
	}
}
