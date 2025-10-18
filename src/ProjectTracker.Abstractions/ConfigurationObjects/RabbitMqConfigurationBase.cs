
namespace ProjectTracker.Abstractions.ConfigurationObjects;

public abstract class RabbitMqConfigurationBase
{
	public required string Host { get; set; }
	public required string VirtualHost { get; set; }
	public required string Username { get; set; }
	public required string Password { get; set; }
}