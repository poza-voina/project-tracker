namespace ProjectTracker.Abstractions.Constants;

//TODO надо перенести как-то Abstractions не должны зависить от объекта
public class EnvironmentConstants
{
	public const string ConnectionSection = "ConnectionStrings";
	public const string DefaultConnectionStringKey = "PostgreSqlConnection";
	public const string RabbitMqKey = "RabbitMQ";
	public const string MinioKey = "Minio";
	public const string ProjectTrackerClientKey = "ProjectTrackerClient";
}