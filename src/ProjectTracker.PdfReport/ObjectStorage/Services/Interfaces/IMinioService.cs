using ProjectTracker.PdfReport.ObjectStorage.Data;

namespace ProjectTracker.PdfReport.ObjectStorage.Services.Interfaces;

public interface IMinioService
{
	Task UploadFileAsync(BucketConfiguration bucketConfiguration, string objectName, byte[] file);
	Task<string> GetFileUrl(BucketConfiguration bucketConfiguration, string objectName, int expirySeconds);
}
