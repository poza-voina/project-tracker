
namespace ProjectTracker.Abstractions.ConfigurationObjects;

public class RabbitMqConfiguration
{
	public required string Host { get; set; }
	public required string VirtualHost { get; set; }
	public required string Username { get; set; }
	public required string Password { get; set; }
	public required RabbitMqTopicBinding HistoryEndpoint { get; set; }
	public required RabbitMqFanoutBinding DefaultEndpoint { get; set; }
}