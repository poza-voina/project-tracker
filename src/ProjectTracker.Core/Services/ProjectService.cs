using Mapster;
using Microsoft.EntityFrameworkCore;
using ProjectTracker.Abstractions.Exceptions;
using ProjectTracker.Contracts.ViewModels.Project;
using ProjectTracker.Contracts.ViewModels.Shared.Pagination;
using ProjectTracker.Core.Extensions;
using ProjectTracker.Core.Services.Interfaces;
using ProjectTracker.Infrastructure.Models;
using ProjectTracker.Infrastructure.Repositories.Interfaces;

namespace ProjectTracker.Core.Services;

public class ProjectService(
	IRepository<ProjectModel> projectRepository,
	IRepository<TaskModel> taskRepository) : IProjectService
{
	public async Task<ProjectBaseResponse> CreateAsync(CreateProjectRequest request)
	{
		var model = request.Adapt<ProjectModel>();

		model = await projectRepository.AddAsync(model);

		return model.Adapt<ProjectBaseResponse>();
	}

	public async Task DeleteAsync(long id)
	{
		var project = await projectRepository
			.GetAll()
			.Include(x => x.TaskFlow)
			.FirstOrDefaultAsync(x => x.Id == id) ?? throw new NotFoundException($"Проект с id = {id} не найден");

		var deletableStatusId = project.TaskFlow!.ProjectDeletableStatusId;

		var statusIds = await taskRepository
			.GetAll()
			.Where(x => x.ProjectId == id)
			.Select(x => x.TaskFlowNodeId)
			.ToListAsync();

		if (!statusIds.All(x => x == deletableStatusId))
		{
			throw new UnprocessableException($"Не все задачи имеют статус с id = {deletableStatusId}");
		}

		await projectRepository.DeleteAsync(id);
	}

	public async Task<PaginationResponse<ProjectBaseResponse>> GetAllAsync(PaginationRequest request)
	{
		var query = projectRepository.GetAll();

		var totalPages = await query.GetTotalPagesAsync(request.PageSize);

		var values = await query.Paginate(request.PageNumber, request.PageSize).ToListAsync();

		return new PaginationResponse<ProjectBaseResponse>
		{
			Values = values.Adapt<IEnumerable<ProjectBaseResponse>>(),
			TotalPages = totalPages
		};
	}

	public async Task<ProjectBaseResponse> GetAsync(long id)
	{
		var model = await projectRepository.FindAsync(id);

		return model.Adapt<ProjectBaseResponse>();
	}

	public async Task<ProjectBaseResponse> UpdateAsync(UpdateProjectRequest request)
	{
		var model = await projectRepository.FindAsync(request.Id);

		request.Adapt(model);

		model = await projectRepository.UpdateAsync(model);

		return model.Adapt<ProjectBaseResponse>();
	}
}
