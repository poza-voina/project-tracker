using MassTransit;
using MassTransit.Initializers;
using Microsoft.EntityFrameworkCore;
using ProjectTracker.Contracts.Events.HistoryEvents;
using ProjectTracker.History.Core.Services.Interfaces;
using ProjectTracker.History.Infrastructure.Models;
using ProjectTracker.History.Infrastructure.Repositories.Interfaces;

namespace ProjectTracker.History.Api.ObjectStorate.Consumers;

public class HistoryConsumer(ITaskHistoryService taskHistoryService) : IConsumer<HistoryEvent>
{
	public async Task Consume(ConsumeContext<HistoryEvent> context)
	{
		var message = context.Message;
		var taskId = message.Meta.Id;

		await taskHistoryService.ProcessHistoryEventAsync(message);

	}
}
