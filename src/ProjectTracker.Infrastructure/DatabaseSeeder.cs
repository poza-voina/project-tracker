using Microsoft.EntityFrameworkCore;
using ProjectTracker.Infrastructure.Enums;
using ProjectTracker.Infrastructure.Models;

namespace ProjectTracker.Infrastructure;

public static class DatabaseSeeder
{
	public static async Task TrySeedAsync(ApplicationDbContext dbContext, CancellationToken cancellationToken = default)
	{
		try
		{
			await ExecuteBody(dbContext, cancellationToken);
		}
		catch (Exception) { }
	}

	private static async Task ExecuteBody(ApplicationDbContext dbContext, CancellationToken cancellationToken)
	{
		await AddTaskGroups(dbContext, cancellationToken);
		await AddTaskFlow(dbContext, cancellationToken);
		await AddEmployees(dbContext, cancellationToken);
		await AddProjects(dbContext, cancellationToken);
		await AddTasks(dbContext, cancellationToken);
	}

	private static async Task AddTaskFlow(ApplicationDbContext dbContext, CancellationToken cancellationToken)
	{
		if (!await dbContext.TaskFlows.AnyAsync(cancellationToken))
		{
			var defaultFlow = new TaskFlowModel();
			dbContext.TaskFlows.Add(defaultFlow);
			await dbContext.SaveChangesAsync(cancellationToken);

			var backlogNode = new TaskFlowNodeModel
			{
				Id = 1,
				Name = "Бэклог",
				TaskFlowId = defaultFlow.Id,
				Status = TaskFlowNodeStatus.Start
			};
			var currentNode = new TaskFlowNodeModel
			{
				Id = 2,
				Name = "Текущая",
				TaskFlowId = defaultFlow.Id,
				Status = TaskFlowNodeStatus.Start
			};
			var activeNode = new TaskFlowNodeModel
			{
				Id = 3,
				Name = "Активна",
				TaskFlowId = defaultFlow.Id
			};
			var testingNode = new TaskFlowNodeModel
			{
				Id = 4,
				Name = "Тестируется",
				TaskFlowId = defaultFlow.Id
			};
			var doneNode = new TaskFlowNodeModel
			{
				Id = 5,
				Name = "Завершена",
				TaskFlowId = defaultFlow.Id,
				Status = TaskFlowNodeStatus.Final
			};
			var canceledNode = new TaskFlowNodeModel
			{
				Id = 6,
				Name = "Отменена",
				TaskFlowId = defaultFlow.Id,
				Status = TaskFlowNodeStatus.Final
			};

			var edges = new[]
			{
				new TaskFlowEdgeModel { FromNodeId = backlogNode.Id, ToNodeId = currentNode.Id, TaskFlowId = defaultFlow.Id },
				new TaskFlowEdgeModel { FromNodeId = currentNode.Id, ToNodeId = activeNode.Id, TaskFlowId = defaultFlow.Id },
				new TaskFlowEdgeModel { FromNodeId = activeNode.Id, ToNodeId = testingNode.Id, TaskFlowId = defaultFlow.Id },
				new TaskFlowEdgeModel { FromNodeId = testingNode.Id, ToNodeId = activeNode.Id, TaskFlowId = defaultFlow.Id },
				new TaskFlowEdgeModel { FromNodeId = testingNode.Id, ToNodeId = doneNode.Id, TaskFlowId = defaultFlow.Id },
				new TaskFlowEdgeModel { FromNodeId = backlogNode.Id, ToNodeId = canceledNode.Id, TaskFlowId = defaultFlow.Id },
				new TaskFlowEdgeModel { FromNodeId = currentNode.Id, ToNodeId = canceledNode.Id, TaskFlowId = defaultFlow.Id },
				new TaskFlowEdgeModel { FromNodeId = activeNode.Id, ToNodeId = canceledNode.Id, TaskFlowId = defaultFlow.Id },
				new TaskFlowEdgeModel { FromNodeId = testingNode.Id, ToNodeId = canceledNode.Id, TaskFlowId = defaultFlow.Id }
			};

			dbContext.TaskFlowNodes.AddRange(backlogNode, currentNode, activeNode, testingNode, doneNode, canceledNode);
			await dbContext.SaveChangesAsync(cancellationToken);

			dbContext.TaskFlowEdges.AddRange(edges);
			await dbContext.SaveChangesAsync(cancellationToken);

			defaultFlow.ProjectDeletableStatusId = canceledNode.Id;
			await dbContext.SaveChangesAsync(cancellationToken);
		}
	}

	private static async Task AddEmployees(ApplicationDbContext dbContext, CancellationToken cancellationToken)
	{
		if (!await dbContext.Employees.AnyAsync(cancellationToken))
		{
			dbContext.Employees.AddRange(
				new EmployeeModel { FirstName = "Alex", LastName = "Ivanov", Username = "a.ivanov", Role = EmployeeRole.Manager },
				new EmployeeModel { FirstName = "Elena", LastName = "Sidorova", Username = "e.sidorova", Role = EmployeeRole.Analyst },
				new EmployeeModel { FirstName = "Maria", LastName = "Petrova", Username = "m.petrova", Role = EmployeeRole.Developer },
				new EmployeeModel { FirstName = "Ivan", LastName = "Kuznetsov", Username = "i.kuznetsov", Role = EmployeeRole.Tester }
			);
			await dbContext.SaveChangesAsync(cancellationToken);
		}
	}

	private static async Task AddProjects(ApplicationDbContext dbContext, CancellationToken cancellationToken)
	{
		if (!await dbContext.Projects.AnyAsync(cancellationToken))
		{
			var pmId = await dbContext.Employees.Select(e => e.Id).FirstAsync(cancellationToken);
			var flowId = await dbContext.TaskFlows.Select(f => f.Id).FirstAsync(cancellationToken);

			var projectNames = new[]
			{
				"Задачи со всеми типами статусов",
				"Задачи с начальными статусами",
				"Задачи с конечными статусами",
				"Задачи с отменой"
			};

			var projects = projectNames.Select((name, idx) => new ProjectModel
			{
				Id = idx + 1,
				Name = name,
				Description = $"Seeded project: {name}",
				ProjectManagerId = pmId,
				TaskFlowId = flowId
			}).ToArray();

			dbContext.Projects.AddRange(projects);

			await dbContext.SaveChangesAsync(cancellationToken);
		}
	}

	private static async Task AddTaskGroups(ApplicationDbContext dbContext, CancellationToken cancellationToken)
	{
		if (!await dbContext.TaskGroups.AnyAsync(cancellationToken))
		{
			dbContext.TaskGroups.AddRange(
				new TaskGroupModel { Name = "Backlog" },
				new TaskGroupModel { Name = "Sprint 1" }
			);
			await dbContext.SaveChangesAsync(cancellationToken);
		}
	}
    
	private static async Task AddTasks(ApplicationDbContext dbContext, CancellationToken cancellationToken)
    {
		if (await dbContext.Tasks.AnyAsync(cancellationToken))
			return;

		var projects = await dbContext.Projects
			.AsNoTracking()
			.ToListAsync(cancellationToken);

		if (projects.Count == 0)
			return;

		var nodes = await dbContext.TaskFlowNodes
			.AsNoTracking()
			.ToListAsync(cancellationToken);

		long GetNodeId(string name)
		{
			return nodes.First(n => n.Name == name).Id;
		}

		var backlogNodeId = GetNodeId("Бэклог");
		var currentNodeId = GetNodeId("Текущая");
		var activeNodeId = GetNodeId("Активна");
		var testingNodeId = GetNodeId("Тестируется");
		var doneNodeId = GetNodeId("Завершена");

		var now = DateTime.UtcNow;

		var tasksToAdd = new List<TaskModel>();

		foreach (var project in projects)
		{
			tasksToAdd.Add(new TaskModel
			{
				Name = $"[{project.Name}] Бэклог задача",
				Description = "Seeded task in backlog",
				ProjectId = project.Id,
				CreatedAt = now,
				TaskFlowNodeId = backlogNodeId,
				Priority = TaskPriority.Low
			});

			tasksToAdd.Add(new TaskModel
			{
				Name = $"[{project.Name}] Текущая задача",
				Description = "Seeded task in current",
				ProjectId = project.Id,
				CreatedAt = now,
				TaskFlowNodeId = currentNodeId,
				Priority = TaskPriority.Medium
			});

			tasksToAdd.Add(new TaskModel
			{
				Name = $"[{project.Name}] Активная задача",
				Description = "Seeded task in active",
				ProjectId = project.Id,
				CreatedAt = now,
				TaskFlowNodeId = activeNodeId,
				Priority = TaskPriority.High
			});

			tasksToAdd.Add(new TaskModel
			{
				Name = $"[{project.Name}] Тестируемая задача",
				Description = "Seeded task in testing",
				ProjectId = project.Id,
				CreatedAt = now,
				TaskFlowNodeId = testingNodeId,
				Priority = TaskPriority.Critical
			});

			tasksToAdd.Add(new TaskModel
			{
				Name = $"[{project.Name}] Завершенная задача",
				Description = "Seeded task in done",
				ProjectId = project.Id,
				CreatedAt = now,
				TaskFlowNodeId = doneNodeId,
				Priority = TaskPriority.Blocker
			});
		}

		dbContext.Tasks.AddRange(tasksToAdd);
		await dbContext.SaveChangesAsync(cancellationToken);
    }
}