using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using ProjectTracker.Infrastructure.Enums;
using ProjectTracker.Infrastructure.Models;

namespace ProjectTracker.Infrastructure;

public static class DatabaseSeeder
{
	private const string _backlog = "Бэклог";
	private const string _current = "Текущая";
	private const string _active = "Активна";
	private const string _test = "Тестируется";
	private const string _end = "Завершена";
	private const string _allStatus = "Задачи со всеми типами статусов";
	private const string _startStatus = "Задачи с начальными статусами";
	private const string _endStatus = "Задачи с конечными статусами";
	private const string _cancelStatus = "Задачи с отменой";

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

		await dbContext.Database.ExecuteSqlRawAsync(
			@"select setval('employee_id_seq', 50);
			select setval('project_id_seq', 50);
			select setval('task_flow_edge_id_seq', 50);
			select setval('task_flow_id_seq', 50);
			select setval('task_flow_node_id_seq', 50);
			select setval('task_group_id_seq', 50);
			select setval('task_id_seq', 50);");
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
				Name = _backlog,
				TaskFlowId = defaultFlow.Id,
				Status = TaskFlowNodeStatus.Start
			};
			var currentNode = new TaskFlowNodeModel
			{
				Id = 2,
				Name = _current,
				TaskFlowId = defaultFlow.Id,
				Status = TaskFlowNodeStatus.Start
			};
			var activeNode = new TaskFlowNodeModel
			{
				Id = 3,
				Name = _active,
				TaskFlowId = defaultFlow.Id
			};
			var testingNode = new TaskFlowNodeModel
			{
				Id = 4,
				Name = _test,
				TaskFlowId = defaultFlow.Id
			};
			var doneNode = new TaskFlowNodeModel
			{
				Id = 5,
				Name = _end,
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
			var employees = new List<EmployeeModel>();

			employees.AddRange(new[]
			{
				new EmployeeModel { FirstName = "Alex", LastName = "Ivanov", Username = "a.ivanov", Role = EmployeeRole.Manager },
				new EmployeeModel { FirstName = "Dmitry", LastName = "Petrov", Username = "d.petrov", Role = EmployeeRole.Manager },
				new EmployeeModel { FirstName = "Anna", LastName = "Smirnova", Username = "a.smirnova", Role = EmployeeRole.Manager },
				new EmployeeModel { FirstName = "Sergey", LastName = "Volkov", Username = "s.volkov", Role = EmployeeRole.Manager },
				new EmployeeModel { FirstName = "Olga", LastName = "Morozova", Username = "o.morozova", Role = EmployeeRole.Manager }
			});

			employees.AddRange(new[]
			{
				new EmployeeModel { FirstName = "Elena", LastName = "Sidorova", Username = "e.sidorova", Role = EmployeeRole.Analyst },
				new EmployeeModel { FirstName = "Vladimir", LastName = "Kozlov", Username = "v.kozlov", Role = EmployeeRole.Analyst },
				new EmployeeModel { FirstName = "Tatiana", LastName = "Novikova", Username = "t.novikova", Role = EmployeeRole.Analyst },
				new EmployeeModel { FirstName = "Andrey", LastName = "Fedorov", Username = "a.fedorov", Role = EmployeeRole.Analyst },
				new EmployeeModel { FirstName = "Natalia", LastName = "Orlova", Username = "n.orlova", Role = EmployeeRole.Analyst }
			});

			employees.AddRange(new[]
			{
				new EmployeeModel { FirstName = "Maria", LastName = "Petrova", Username = "m.petrova", Role = EmployeeRole.Developer },
				new EmployeeModel { FirstName = "Pavel", LastName = "Sokolov", Username = "p.sokolov", Role = EmployeeRole.Developer },
				new EmployeeModel { FirstName = "Ekaterina", LastName = "Lebedeva", Username = "e.lebedeva", Role = EmployeeRole.Developer },
				new EmployeeModel { FirstName = "Maxim", LastName = "Popov", Username = "m.popov", Role = EmployeeRole.Developer },
				new EmployeeModel { FirstName = "Svetlana", LastName = "Vasilieva", Username = "s.vasilieva", Role = EmployeeRole.Developer }
			});

			employees.AddRange(new[]
			{
				new EmployeeModel { FirstName = "Ivan", LastName = "Kuznetsov", Username = "i.kuznetsov", Role = EmployeeRole.Tester },
				new EmployeeModel { FirstName = "Yulia", LastName = "Stepanova", Username = "y.stepanova", Role = EmployeeRole.Tester },
				new EmployeeModel { FirstName = "Roman", LastName = "Mikhailov", Username = "r.mikhailov", Role = EmployeeRole.Tester },
				new EmployeeModel { FirstName = "Larisa", LastName = "Fomina", Username = "l.fomina", Role = EmployeeRole.Tester },
				new EmployeeModel { FirstName = "Denis", LastName = "Grigoriev", Username = "d.grigoriev", Role = EmployeeRole.Tester }
			});

			dbContext.Employees.AddRange(employees);
			await dbContext.SaveChangesAsync(cancellationToken);
		}
	}

	private static async Task AddProjects(ApplicationDbContext dbContext, CancellationToken cancellationToken)
	{
		if (!await dbContext.Projects.AnyAsync(cancellationToken))
		{
			var pmId = await dbContext.Employees.Select(e => e.Id).FirstAsync(cancellationToken);
			var flowId = await dbContext.TaskFlows.Select(f => f.Id).FirstAsync(cancellationToken);

			var _names = GetNames();

			var projects = _names.Select((name, idx) => new ProjectModel
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

	private static IEnumerable<string> GetNames() =>
		[
			_allStatus,
			_startStatus,
			_endStatus,
			_cancelStatus
		];

	private static async Task AddTaskGroups(ApplicationDbContext dbContext, CancellationToken cancellationToken)
	{

		var _names = GetNames();

		if (!await dbContext.TaskGroups.AnyAsync(cancellationToken))
		{
			dbContext.TaskGroups.AddRange(
				_names.Select(x => new TaskGroupModel { Name = x }).ToList()
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

		var groups = await dbContext
			.TaskGroups
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

		var backlogNodeId = GetNodeId(_backlog);
		var currentNodeId = GetNodeId(_current);
		var activeNodeId = GetNodeId(_active);
		var testingNodeId = GetNodeId(_test);
		var doneNodeId = GetNodeId(_end);

		var tasksToAdd = new List<TaskModel>();

		var project = projects.First(x => x.Name == _allStatus);
		var group = groups.First(x => x.Name == _allStatus);
		tasksToAdd.Add(new TaskModel
		{
			Name = $"[{project.Name}] Бэклог задача",
			Description = "Seeded task in backlog",
			ProjectId = project.Id,
			GroupId = group.Id,
			TaskFlowNodeId = backlogNodeId,
			Priority = TaskPriority.Low
		});

		tasksToAdd.Add(new TaskModel
		{
			Name = $"[{project.Name}] Текущая задача",
			Description = "Seeded task in current",
			ProjectId = project.Id,
			GroupId = group.Id,
			TaskFlowNodeId = currentNodeId,
			Priority = TaskPriority.Medium
		});

		tasksToAdd.Add(new TaskModel
		{
			Name = $"[{project.Name}] Активная задача",
			Description = "Seeded task in active",
			ProjectId = project.Id,
			GroupId = group.Id,
			TaskFlowNodeId = activeNodeId,
			Priority = TaskPriority.High
		});

		tasksToAdd.Add(new TaskModel
		{
			Name = $"[{project.Name}] Тестируемая задача",
			Description = "Seeded task in testing",
			ProjectId = project.Id,
			GroupId = group.Id,
			TaskFlowNodeId = testingNodeId,
			Priority = TaskPriority.Critical
		});

		tasksToAdd.Add(new TaskModel
		{
			Name = $"[{project.Name}] Завершенная задача",
			Description = "Seeded task in done",
			ProjectId = project.Id,
			GroupId = group.Id,
			TaskFlowNodeId = doneNodeId,
			Priority = TaskPriority.Blocker
		});

		project = projects.FirstOrDefault(x => x.Name == _startStatus);
		group = groups.FirstOrDefault(x => x.Name == _startStatus);
		if (project is { } && group is { })
		{
			tasksToAdd.Add(new TaskModel
			{
				Name = $"[{project.Name}] Задача в бэклоге 1",
				Description = "Первая задача в бэклоге",
				ProjectId = project.Id,
				GroupId = group.Id,
				TaskFlowNodeId = backlogNodeId,
				Priority = TaskPriority.Low
			});

			tasksToAdd.Add(new TaskModel
			{
				Name = $"[{project.Name}] Задача в бэклоге 2",
				Description = "Вторая задача в бэклоге",
				ProjectId = project.Id,
				GroupId = group.Id,
				TaskFlowNodeId = backlogNodeId,
				Priority = TaskPriority.Medium
			});

			tasksToAdd.Add(new TaskModel
			{
				Name = $"[{project.Name}] Текущая задача 1",
				Description = "Первая текущая задача",
				ProjectId = project.Id,
				GroupId = group.Id,
				TaskFlowNodeId = currentNodeId,
				Priority = TaskPriority.High
			});

			tasksToAdd.Add(new TaskModel
			{
				Name = $"[{project.Name}] Текущая задача 2",
				Description = "Вторая текущая задача",
				ProjectId = project.Id,
				GroupId = group.Id,
				TaskFlowNodeId = currentNodeId,
				Priority = TaskPriority.Critical
			});

			tasksToAdd.Add(new TaskModel
			{
				Name = $"[{project.Name}] Активная задача",
				Description = "Активная задача",
				ProjectId = project.Id,
				GroupId = group.Id,
				TaskFlowNodeId = activeNodeId,
				Priority = TaskPriority.Blocker
			});
		}

		project = projects.FirstOrDefault(x => x.Name == _endStatus);
		group = groups.FirstOrDefault(x => x.Name == _endStatus);
		if (project is { } && group is { })
		{
			tasksToAdd.Add(new TaskModel
			{
				Name = $"[{project.Name}] Завершенная задача 1",
				Description = "Первая завершенная задача",
				ProjectId = project.Id,
				GroupId = group.Id,
				TaskFlowNodeId = doneNodeId,
				Priority = TaskPriority.Low
			});

			tasksToAdd.Add(new TaskModel
			{
				Name = $"[{project.Name}] Завершенная задача 2",
				Description = "Вторая завершенная задача",
				ProjectId = project.Id,
				GroupId = group.Id,
				TaskFlowNodeId = doneNodeId,
				Priority = TaskPriority.Medium
			});

			tasksToAdd.Add(new TaskModel
			{
				Name = $"[{project.Name}] Завершенная задача 3",
				Description = "Третья завершенная задача",
				ProjectId = project.Id,
				GroupId = group.Id,
				TaskFlowNodeId = doneNodeId,
				Priority = TaskPriority.High
			});

			tasksToAdd.Add(new TaskModel
			{
				Name = $"[{project.Name}] Тестируемая задача 1",
				Description = "Первая тестируемая задача",
				ProjectId = project.Id,
				GroupId = group.Id,
				TaskFlowNodeId = testingNodeId,
				Priority = TaskPriority.Critical
			});

			tasksToAdd.Add(new TaskModel
			{
				Name = $"[{project.Name}] Тестируемая задача 2",
				Description = "Вторая тестируемая задача",
				ProjectId = project.Id,
				GroupId = group.Id,
				TaskFlowNodeId = testingNodeId,
				Priority = TaskPriority.Blocker
			});
		}

		project = projects.FirstOrDefault(x => x.Name == _cancelStatus);
		group = groups.FirstOrDefault(x => x.Name == _cancelStatus);
		if (project is { } && group is { })
		{
			var canceledNodeId = GetNodeId("Отменена");

			tasksToAdd.Add(new TaskModel
			{
				Name = $"[{project.Name}] Отмененная задача 1",
				Description = "Первая отмененная задача",
				ProjectId = project.Id,
				GroupId = group.Id,
				TaskFlowNodeId = canceledNodeId,
				Priority = TaskPriority.Low
			});

			tasksToAdd.Add(new TaskModel
			{
				Name = $"[{project.Name}] Отмененная задача 2",
				Description = "Вторая отмененная задача",
				ProjectId = project.Id,
				GroupId = group.Id,
				TaskFlowNodeId = canceledNodeId,
				Priority = TaskPriority.Medium
			});

			tasksToAdd.Add(new TaskModel
			{
				Name = $"[{project.Name}] Отмененная задача 3",
				Description = "Третья отмененная задача",
				ProjectId = project.Id,
				GroupId = group.Id,
				TaskFlowNodeId = canceledNodeId,
				Priority = TaskPriority.High
			});

			tasksToAdd.Add(new TaskModel
			{
				Name = $"[{project.Name}] Отмененная задача 4",
				Description = "Четвертая отмененная задача",
				ProjectId = project.Id,
				GroupId = group.Id,
				TaskFlowNodeId = canceledNodeId,
				Priority = TaskPriority.Critical
			});

			tasksToAdd.Add(new TaskModel
			{
				Name = $"[{project.Name}] Отмененная задача 5",
				Description = "Пятая отмененная задача",
				ProjectId = project.Id,
				GroupId = group.Id,
				TaskFlowNodeId = canceledNodeId,
				Priority = TaskPriority.Blocker
			});
		}

		dbContext.Tasks.AddRange(tasksToAdd);
		await dbContext.SaveChangesAsync(cancellationToken);
	}
}