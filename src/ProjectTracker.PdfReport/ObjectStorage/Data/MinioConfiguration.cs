namespace ProjectTracker.PdfReport.ObjectStorage.Data;

public class MinioConfiguration
{
	public required BucketConfiguration ReportBucket { get; init; }
	public required string Endpoint { get; init; }
	public required string AccessKey { get; init; }
	public required string SecretKey { get; init; }
}
