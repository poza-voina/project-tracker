using Mapster;
using Microsoft.EntityFrameworkCore;
using ProjectTracker.Abstractions.Exceptions;
using ProjectTracker.Contracts.ViewModels.Shared.Pagination;
using ProjectTracker.Contracts.ViewModels.Task;
using ProjectTracker.Core.Extensions;
using ProjectTracker.Core.Services.Interfaces;
using ProjectTracker.Infrastructure.Enums;
using ProjectTracker.Infrastructure.Models;
using ProjectTracker.Infrastructure.Repositories.Interfaces;

namespace ProjectTracker.Core.Services;

public class TaskService(
	IRepository<TaskModel> taskRepository,
	IRepository<TaskFlowEdgeModel> edgeRepository,
	IRepository<PerformerTaskModel> performerRepository,
	IRepository<ObserverTaskModel> observerRepository,
	IRepository<ProjectModel> projectRepository) : ITaskService
{
	public async Task AddObserverAsync(AddTaskObserverRequest request)
	{
		await observerRepository.AddAsync(request.Adapt<ObserverTaskModel>());
	}

	public async Task AddPerformerAsync(AddTaskPerformerRequest request)
	{
		await performerRepository.AddAsync(request.Adapt<PerformerTaskModel>());
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

		taskModel.TaskFlowNodeId = toEdge.ToNode!.Id; //TODO !!!
		await taskRepository.UpdateAsync(taskModel);

		taskModel.Status = toEdge.ToNode;
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

		return model.Adapt<TaskWithStatusResponse>();
	}

	public async Task DeleteAsync(long id)
	{
		var taskModel = await taskRepository
			.GetAll()
			.Include(x => x.Status)
			.FirstOrDefaultAsync(x => x.Id == id)
			?? throw new NotFoundException($"Задача с id = {id} не найдена");

		if (taskModel.Status!.Status != TaskFlowNodeStatus.Final) //TODO !!!!
		{
			throw new UnprocessableException("Невозможно удалить задачу не в конечном статусе");
		}

		await taskRepository.DeleteAsync(id);
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

	public async Task<TaskWithStatusEmployeesReponse> GetAsync(long id)
	{
		var model = await taskRepository
			.GetAll()
			.Include(x => x.Status)
			.Include(x => x.Observers)
			.Include(x => x.Performers)
			.FirstOrDefaultAsync(x => x.Id == id)
			?? throw new NotFoundException($"Задача с id = {id} не найдена");

		return model.Adapt<TaskWithStatusEmployeesReponse>();
	}
}
