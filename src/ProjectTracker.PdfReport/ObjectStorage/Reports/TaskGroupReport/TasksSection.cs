namespace ProjectTracker.PdfReport.ObjectStorage.Reports.TaskGroupReport;

using ProjectTracker.PdfReport.ObjectStorage.Dtos.TaskGroupReport;
using ProjectTracker.PdfReport.ObjectStorage.FieldBuilder;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

public class TaskSection
{
	private readonly TaskGroupTaskDto _task;

	public TaskSection(TaskGroupTaskDto task)
	{
		_task = task;
	}

	public void Compose(IContainer container)
	{
		container.ShowEntire().Column(column =>
			{
				column
					.Item()
					.Element(
						container => container.GenerateHeaderSection($"Информация о задаче {_task.Id}")
					);

				var taskProperties = new FieldsBuilder<TaskGroupTaskDto>()
					.BuildWithPrimitiveFields(_task);

				if (taskProperties.Any())
				{
					column
						.Item()
						.Element(
							container =>
								GenerateReportHelper.GenerateTableTypeFirst(container, taskProperties)
						);
				}
				else
				{
					column
						.Item()
						.Element(container => container.GenerateNotFoundMessage("Информация о задачи не найдена"));
				}


				column
					.Item()
					.Element(container => container.GenerateHeaderSection("Исполнители")
				);

				var performerProperties = new ManyFieldsBuilder<TaskGroupEmployeeDto>()
					.BuildWithPrimitiveFields(_task.Performers);

				if (performerProperties.Count > 0)
				{
					column
						.Item()
						.Element(
							container =>
								container.GenerateTableTypeSecond(performerProperties)
						);
				}
				else
				{
					column
						.Item()
						.Element(
							container => container.GenerateNotFoundMessage("Исполнители не найдены")
							);
				}
			}
		);
	}
}
