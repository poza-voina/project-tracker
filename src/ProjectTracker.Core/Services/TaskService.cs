using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using ProjectTracker.Abstractions.Exceptions;
using ProjectTracker.Contracts.Events.HistoryEvents;
using ProjectTracker.Contracts.ViewModels.Shared.Pagination;
using ProjectTracker.Contracts.ViewModels.Task;
using ProjectTracker.Core.Extensions;
using ProjectTracker.Core.ObjectStorage;
using ProjectTracker.Core.ObjectStorage.Dtos.History.Task;
using ProjectTracker.Core.ObjectStorage.Interfaces;
using ProjectTracker.Core.Services.Interfaces;
using ProjectTracker.Infrastructure.Enums;
using ProjectTracker.Infrastructure.Models;
using ProjectTracker.Infrastructure.Repositories.Interfaces;
using System.Diagnostics.CodeAnalysis;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ProjectTracker.Core.Services;

public class TaskService(
	IRepository<TaskModel> taskRepository,
	IRepository<TaskFlowEdgeModel> edgeRepository,
	IRepository<PerformerTaskModel> performerRepository,
	IRepository<ObserverTaskModel> observerRepository,
	IRepository<ProjectModel> projectRepository,
	IHistoryEventProcessor historyEventProcessor,
	IEventCollector eventCollector) : ITaskService
{
	public async Task AddObserverAsync(AddTaskObserverRequest request)
	{
		List<long> existingIds = await GetObserverIdsAsync(request.TaskId);

		var isObserverExists = existingIds.Any(x => x == request.EmployeeId);

		var isPerformerExists = await performerRepository
			.GetAll()
			.AnyAsync(x => x.TaskId == request.TaskId && x.EmployeeId == request.EmployeeId);

		if (isObserverExists || isPerformerExists)
		{
			throw new BadRequestException("Сотрудник может быть либо наблюдателем либо исполнителем");
		}

		await observerRepository.AddAsync(request.Adapt<ObserverTaskModel>());

		var @event = historyEventProcessor.CreateHistoryEventOrDefault(
			new TaskObserversDto { ObserverIds = existingIds },
			new TaskObserversDto { ObserverIds = existingIds.Append(request.EmployeeId).ToList() },
			HistoryEventMeta.CreateMeta(request.TaskId, HistoryEventType.TaskAddObserver)
		);

		eventCollector.Add(@event);
	}

	private async Task<List<long>> GetObserverIdsAsync(long taskId)
	{
		return await observerRepository
			.GetAll()
			.Where(x => x.TaskId == taskId)
			.Select(x => x.EmployeeId)
			.ToListAsync();
	}

	private async Task<List<long>> GetPerformerIdsAsync(long taskId)
	{
		return await performerRepository
			.GetAll()
			.Where(x => x.TaskId == taskId)
			.Select(x => x.EmployeeId)
			.ToListAsync();
	}

	public async Task AddPerformerAsync(AddTaskPerformerRequest request)
	{
		var existingIds = await GetPerformerIdsAsync(request.TaskId);

		var isPerformerExists = existingIds.Any(x => x == request.EmployeeId);

		var isObserverExists = await observerRepository
			.GetAll()
			.AnyAsync(x => x.TaskId == request.TaskId && x.EmployeeId == request.EmployeeId);

		if (isObserverExists || isPerformerExists)
		{
			throw new BadRequestException("Сотрудник может быть либо наблюдателем либо исполнителем");
		}

		await performerRepository.AddAsync(request.Adapt<PerformerTaskModel>());

		var @event = historyEventProcessor.CreateHistoryEventOrDefault(
			new TaskPerformersDto { PerformerIds = existingIds },
			new TaskPerformersDto { PerformerIds = existingIds.Append(request.EmployeeId).ToList() },
			HistoryEventMeta.CreateMeta(request.TaskId, HistoryEventType.TaskAddPerformer)
		);

		eventCollector.Add(@event);
	}

	public async Task<TaskWithStatusEmployeesReponse> ChangeStatusAsync(ChangeTaskStatusRequest request)
	{
		var taskModel = await taskRepository
			.GetAll()
			.Include(x => x.Status)
			.FirstOrDefaultAsync(x => x.Id == request.TaskId)
			?? throw new NotFoundException($"Задача с id = {request.TaskId} не найдена");

		if (taskModel.Version != request.TaskVersion)
		{
			throw new DbUpdateConcurrencyException();
		}

		var toEdge = await edgeRepository.GetAll()
			.Include(x => x.ToNode)
			.FirstOrDefaultAsync(
				x => x.FromNodeId == taskModel.TaskFlowNodeId &&
				x.ToNodeId == request.TaskFlowNodeId)
			?? throw new NotFoundException("Статус не найден или не является следующим");

		var old = new TaskStatusHistoryDto { TaskFlowNodeId = taskModel.TaskFlowNodeId };

		taskModel.TaskFlowNodeId = toEdge.ToNode!.Id; //TODO !!!

		var current = new TaskStatusHistoryDto { TaskFlowNodeId = toEdge.ToNode!.Id };

		await taskRepository.UpdateAsync(taskModel);

		taskModel.Status = toEdge.ToNode;

		var @event = historyEventProcessor.CreateHistoryEventOrDefault(
				old, current,
				HistoryEventMeta.CreateMeta(request.TaskId, HistoryEventType.TaskChangeStatus)
			);

		eventCollector.Add(@event);

		return taskModel.Adapt<TaskWithStatusEmployeesReponse>();
	}

	public async Task<TaskWithStatusResponse> CreateAsync(CreateTaskRequest request)
	{
		var model = request.Adapt<TaskModel>();
		var taskId = (await taskRepository.AddAsync(model)).Id;

		var nodeIds = await projectRepository
			.GetAll()
			.Where(x => x.Id == request.ProjectId)
			.Include(x => x.TaskFlow)
			.ThenInclude(x => x!.Nodes) //TODO !!!
			.SelectMany(x => x.TaskFlow!.Nodes) //TODO !!!
			.Where(x => x.Status == TaskFlowNodeStatus.Start)
			.Select(x => x.Id)
			.ToListAsync();

		if (!nodeIds.Any(x => x == request.TaskFlowNodeId))
		{
			throw new UnprocessableException($"Статуса с id = {request.TaskFlowNodeId} не существует или он не начальный");
		}

		model = taskRepository
			.GetAll()
			.Include(x => x.Status)
			.First(x => x.Id == taskId);

		var @event = historyEventProcessor
			.CreateHistoryEventOrDefault(
				null,
				model.Adapt<TaskHistoryDto>(),
				HistoryEventMeta.CreateMeta(model.Id, HistoryEventType.TaskCreate)
			);

		eventCollector.Add(@event);

		return model.Adapt<TaskWithStatusResponse>();
	}

	public async Task DeleteAsync(DeleteTaskRequest request)
	{
		var taskModel = await taskRepository
			.GetAll()
			.Include(x => x.Status)
			.FirstOrDefaultAsync(x => x.Id == request.Id)
			?? throw new NotFoundException($"Задача с id = {request.Id} не найдена");

		if (taskModel.Status!.Status != TaskFlowNodeStatus.Final) //TODO !!!!
		{
			throw new UnprocessableException("Невозможно удалить задачу не в конечном статусе");
		}

		var @event = historyEventProcessor
			.CreateHistoryEventOrDefault(
				taskModel.Adapt<TaskHistoryDto>(),
				null,
				HistoryEventMeta.CreateMeta(taskModel.Id, HistoryEventType.TaskDelete)
			);

		eventCollector.Add(@event);

		await taskRepository.DeleteAsync(request.Id);
	}

	public async Task<PaginationResponse<TaskWithStatusEmployeesReponse>> GetAllAsync(GetPaginationTasksRequest request)
	{
		var query = taskRepository
			.GetAll()
			.Include(x => x.Status)
			.Include(x => x.Observers)
			.Include(x => x.Performers)
			.AsQueryable();

		if (request.ProjectId is { })
		{
			query = query.Where(x => x.ProjectId == request.ProjectId);
		}
		if (request.GroupId is { })
		{
			query = query.Where(x => x.GroupId == request.GroupId);
		}
		if (request.EmployeeId is { })
		{
			query = query.Where(
				x => x.Observers!.Any(x => x.Id == request.EmployeeId) ||
				x.Performers!.Any(x => x.Id == request.EmployeeId)); //TODO !!!
		}

		var totalPages = await query.GetTotalPagesAsync(request.PageSize);
		var values = await query.Paginate(request.PageNumber, request.PageSize).ToListAsync();

		return new PaginationResponse<TaskWithStatusEmployeesReponse>
		{
			TotalPages = totalPages,
			Values = values.Adapt<IEnumerable<TaskWithStatusEmployeesReponse>>()
		};
	}

	public async Task<TaskWithStatusEmployeesReponse> GetAsync(GetTaskRequest request)
	{
		var model = await taskRepository
			.GetAll()
			.Include(x => x.Status)
			.Include(x => x.Observers)
			.Include(x => x.Performers)
			.FirstOrDefaultAsync(x => x.Id == request.Id)
			?? throw new NotFoundException($"Задача с id = {request.Id} не найдена");

		return model.Adapt<TaskWithStatusEmployeesReponse>();
	}

	public async Task<TaskWithStatusResponse> UpdateAsync(UpdateTaskRequest request)
	{
		var model = await taskRepository
			.GetAll()
			.Include(x => x.Status)
			.FirstOrDefaultAsync(x => x.Id == request.Id)
			?? throw new NotFoundException($"Задача с id = {request.Id} не найдена");

		var old = model.Adapt<TaskHistoryDto>();

		request.Adapt(model);

		model = await taskRepository.UpdateAsync(model);

		var current = model.Adapt<TaskHistoryDto>();

		eventCollector.Add(historyEventProcessor.CreateHistoryEventOrDefault(old, current, HistoryEventMeta.CreateMeta(model.Id, HistoryEventType.TaskUpdate)));

		return model.Adapt<TaskWithStatusResponse>();
	}

	public async Task<TaskReportInformationResponse> GetReportInformationAsync(GetTaskReportRequest request)
	{
		var taskId = request.Id;

		var task = await taskRepository
			.GetAll()
			.Include(x => x.Project)
			.Include(x => x.TaskGroup)
			.Include(x => x.Performers)
			.Include(x => x.Observers)
			.FirstOrDefaultAsync(x => x.Id == taskId)
			?? throw new NotFoundException($"Задача не найдена с id = {taskId}");

		var project = await projectRepository
			.GetAll()
			.Include(x => x.ProjectManager)
			.Include(x => x.Manager)
			.FirstOrDefaultAsync(x => x.Id == task.ProjectId)
			?? throw new NotFoundException($"Проект не найден c id = {task.ProjectId}");

		task.Project = project;

		return task.Adapt<TaskReportInformationResponse>();
	}
}
