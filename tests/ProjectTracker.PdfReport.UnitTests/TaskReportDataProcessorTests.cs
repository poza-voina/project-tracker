using FluentAssertions;
using ProjectTracker.Infrastructure.Enums;
using ProjectTracker.PdfReport.ObjectStorage;
using ProjectTracker.PdfReport.ObjectStorage.Dataproviders;
using ProjectTracker.PdfReport.ObjectStorage.Dtos.TaskGroupReport;
using ProjectTracker.PdfReport.ObjectStorage.Dtos.TaskReport;
using ProjectTracker.PdfReport.ObjectStorage.FieldBuilder;
using System.Text.Json;
using Xunit;

namespace ProjectTracker.PdfReport.UnitTests;

public class TaskReportDataProcessorTests
{
	[Fact]
	public void Test()
	{
		var data = new TaskReportDto
		{
			Id = 64,
			Name = "lorem ipsum",
			Description = "lorem ipsum",
			Deadline = DateTime.Now,
			CreatedAt = DateTime.Now,
			Priority = TaskPriority.Low,
			Version = 32U,
			Project = new TaskReportProjectDto
			{
				Id = 64,
				Name = "lorem ipsum",
				ProjectManager = new TaskReportEmployeeDto
				{
					Id = 64,
					LastName = "lorem ipsum",
					FirstName = "lorem ipsum",
					Patronymic = "lorem ipsum",
					Username = "lorem ipsum",
					Role = EmployeeRole.Manager
				},
				Manager = new TaskReportEmployeeDto
				{
					Id = 64,
					LastName = "lorem ipsum",
					FirstName = "lorem ipsum",
					Patronymic = "lorem ipsum",
					Username = "lorem ipsum",
					Role = EmployeeRole.Manager
				}
			},
			Group = null,
			Observers = new List<TaskReportEmployeeDto>
			{
				new TaskReportEmployeeDto
				{
					Id = 64,
					LastName = "lorem ipsum",
					FirstName = "lorem ipsum",
					Patronymic = "lorem ipsum",
					Username = "lorem ipsum",
					Role = EmployeeRole.Manager
				}
			},
			Performers = new List<TaskReportEmployeeDto>
			{
				new TaskReportEmployeeDto
				{
					Id = 64,
					LastName = "lorem ipsum",
					FirstName = "lorem ipsum",
					Patronymic = "lorem ipsum",
					Username = "lorem ipsum",
					Role = EmployeeRole.Manager
				}
			}
		};

		var props = new FieldsBuilder<TaskReportDto>()
		.Add(
			x => x.Project.Name,
			x => x.Group.Name
		)
		.AsFieldsBuilder()
		.BuildWithPrimitiveFields(data);


		Assert.Fail();
	}
}
