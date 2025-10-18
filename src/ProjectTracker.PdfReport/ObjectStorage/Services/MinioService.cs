using Minio;
using Minio.DataModel.Args;
using ProjectTracker.PdfReport.ObjectStorage.Data;
using ProjectTracker.PdfReport.ObjectStorage.Services.Interfaces;

namespace ProjectTracker.PdfReport.ObjectStorage.Services;

public class MinioService(IMinioClient minioClient) : IMinioService
{
	public async Task UploadFileAsync(BucketConfiguration bucketConfiguration, string objectName, byte[] file)
	{
		using var stream = new MemoryStream(file);

		var putObjectArgs = new PutObjectArgs()
			.WithBucket(bucketConfiguration.BucketName)
			.WithObject(objectName)
			.WithObjectSize(stream.Length)
			.WithContentType(bucketConfiguration.ContentType)
			.WithStreamData(stream);

		await minioClient.PutObjectAsync(putObjectArgs);
	}

	public async Task<string> GetFileUrl(BucketConfiguration bucketConfiguration, string objectName, int expirySeconds)
	{
		var presignedUrl = await minioClient.PresignedGetObjectAsync(
			new PresignedGetObjectArgs()
				.WithBucket(bucketConfiguration.BucketName)
				.WithObject(objectName)
				.WithExpiry(expirySeconds));

		return presignedUrl;
	}
}
