using Mapster;
using Microsoft.EntityFrameworkCore;
using ProjectTracker.Abstractions.Exceptions;
using ProjectTracker.Contracts.Events.HistoryEvents;
using ProjectTracker.History.Contracts.ViewModels.TaskHistory;
using ProjectTracker.History.Core.Services.Interfaces;
using ProjectTracker.History.Infrastructure.Models;
using ProjectTracker.History.Infrastructure.Repositories.Interfaces;
using System.Threading.Tasks;

namespace ProjectTracker.History.Core.Services;

public class TaskHistoryService(
	IRepository<TaskHistoryRecordModel> recordRepository,
	IRepository<TaskHistoryMetaModel> metaRepository
	) : ITaskHistoryService
{
	public async Task<IEnumerable<GetTasksHistoryResponse>> GetAllAsync(GetTasksHistoryRequest request)
	{
		var recordsQuery = recordRepository.GetAll();

		if (request.TaskId is { })
		{
			var meta = await metaRepository
				.GetAll()
				.FirstOrDefaultAsync(x => x.TaskId == request.TaskId)
				?? throw new NotFoundException($"Мета информация для задачи с id = {request.TaskId} не найдена");

			recordsQuery = recordsQuery.Where(x => x.MetaId == meta.Id);
		}

		var records = await recordsQuery.ToListAsync();

		return records.Adapt<IEnumerable<GetTasksHistoryResponse>>();
	}

	public async Task ProcessHistoryEventAsync(HistoryEvent message)
	{
		var taskId = message.Meta.Id;

		var meta = await metaRepository
			.GetAll()
			.FirstOrDefaultAsync(x => x.TaskId == taskId);

		if (meta is null)
		{
			meta = await metaRepository.AddAsync(new TaskHistoryMetaModel { TaskId = taskId });
		}

		var changes = message.Changes
			.Select(x =>
			{
				return new TaskHistoryRecordModel
				{
					Property = x.Property,
					ChangeEventType = message.Meta.EventType.ToString(),
					OldValue = x.OldValue,
					NewValue = x.NewValue,
					MetaId = meta.Id
				};
			});

		await recordRepository.AddRangeAsync(changes);
	}
}
