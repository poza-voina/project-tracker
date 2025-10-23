using Mapster;
using Microsoft.EntityFrameworkCore;
using ProjectTracker.Abstractions.Exceptions;
using ProjectTracker.Contracts.ViewModels.Shared.Pagination;
using ProjectTracker.Contracts.ViewModels.TaskGroup;
using ProjectTracker.Core.Extensions;
using ProjectTracker.Core.Services.Interfaces;
using ProjectTracker.Infrastructure.Enums;
using ProjectTracker.Infrastructure.Models;
using ProjectTracker.Infrastructure.Repositories.Interfaces;

namespace ProjectTracker.Core.Services;

public class GroupService(
	IRepository<TaskGroupModel> groupRepository,
	IRepository<TaskModel> taskRepository) : IGroupService
{
	public async Task<TaskGroupBaseResponse> CreateAsync(CreateTaskGroupRequest request)
	{
		var model = request.Adapt<TaskGroupModel>();

		model = await groupRepository.AddAsync(model);

		return model.Adapt<TaskGroupBaseResponse>();
	}

	public async Task DeleteAsync(DeleteGroupRequest request)
	{
		var tasks = await taskRepository
			.GetAll()
			.Where(x => x.GroupId == request.Id)
			.Include(x => x.Status)
			.ToListAsync();

		var isDelete = tasks.Count == 0 ||
			tasks.All(
				x => x.Status != null &&
				x.Status.Status == TaskFlowNodeStatus.Final);

		if (!isDelete)
		{
			throw new UnprocessableException("Невозможно удалить группу: есть задачи не в конечном статусе");
		}

		await groupRepository.DeleteAsync(request.Id);
	}

	public async Task<PaginationResponse<TaskGroupBaseResponse>> GetAllAsync(PaginationRequest request)
	{
		var query = groupRepository.GetAll();

		var totalPages = await query.GetTotalPagesAsync(request.PageSize);

		var items = await query.Paginate(request.PageNumber, request.PageSize)
			.ToListAsync();

		return new PaginationResponse<TaskGroupBaseResponse>
		{
			TotalPages = totalPages,
			Values = items.Adapt<IEnumerable<TaskGroupBaseResponse>>()
		};
	}

	public async Task<TaskGroupBaseResponse> GetAsync(GetGroupRequest request)
	{
		var model = await groupRepository.FindAsync(request.Id);

		return model.Adapt<TaskGroupBaseResponse>();
	}

	public async Task<TaskGroupInformationResponse> GetReportInformationAsync(GetGroupReportInforamationRequest request)
	{
		var model = await groupRepository
			.GetAll()
			.Include(x => x.Tasks)
				.ThenInclude(x => x.Status)
			.Include(x => x.Tasks)
				.ThenInclude(x => x.Performers)
			.FirstOrDefaultAsync(x => x.Id == request.Id)
			?? throw new NotFoundException($"Группа с id = {request.Id} не найдена");

		return model.Adapt<TaskGroupInformationResponse>();
	}

	public async Task<TaskGroupBaseResponse> UpdateAsync(UpdateTaskGroupRequest request)
	{
		var model = await groupRepository.FindAsync(request.Id);

		request.Adapt(model);

		await groupRepository.UpdateAsync(model);

		return model.Adapt<TaskGroupBaseResponse>();
	}
}