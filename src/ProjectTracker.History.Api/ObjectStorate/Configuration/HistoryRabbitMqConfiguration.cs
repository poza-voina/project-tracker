using MassTransit;
using ProjectTracker.Abstractions.ConfigurationObjects;

namespace ProjectTracker.History.Api.ObjectStorate.Configuration;

public class HistoryRabbitMqConfiguration : RabbitMqConfigurationBase
{
	public required RabbitMqEndpointBinding HistoryEndpoint { get; set; }
}
