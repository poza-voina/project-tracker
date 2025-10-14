namespace ProjectTracker.Abstractions.ConfigurationObjects;

public class RabbitMqTopicBinding
{
	public required string Name { get; set; }
	public required string RoutingKey { get;set; }
}
