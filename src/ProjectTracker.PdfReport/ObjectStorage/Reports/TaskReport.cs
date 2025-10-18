using Mapster;
using ProjectTracker.Contracts.ViewModels.Task;
using ProjectTracker.PdfReport.ObjectStorage.Dtos.TaskReport;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace ProjectTracker.PdfReport.ObjectStorage.Reports;

public class TaskReport : IDocument
{
	private readonly TaskReportDto _taskReportDto;

	private Dictionary<string, string?> taskProperties = new();
	private Dictionary<string, string?> projectManagerProperties;
	private Dictionary<string, string?> managerProperties;
	private Dictionary<string, List<string?>> _observers;
	private Dictionary<string, List<string?>> _performers;

	public TaskReport(TaskReportDto taskReportDto)
	{
		_taskReportDto = taskReportDto;

		taskProperties = new FieldsBuilder()
			.AddAllPrimitiveFields(_taskReportDto)
			.AddPrimitiveFields(_taskReportDto.Project, nameof(_taskReportDto.Project.Name))
			.AddPrimitiveFields(_taskReportDto.Group, nameof(_taskReportDto.Group.Name))
			.GetFields();

		projectManagerProperties = new FieldsBuilder()
			.AddAllPrimitiveFields(_taskReportDto.Project.ProjectManager)
			.GetFields();

		managerProperties = new FieldsBuilder()
			.AddAllPrimitiveFields(_taskReportDto.Project.Manager)
			.GetFields();

		_observers = new ManyFieldsBuilder()
			.AddAllPrimitiveFields(_taskReportDto.Observers)
			.GetFields();

		_performers = new ManyFieldsBuilder()
			.AddAllPrimitiveFields(_taskReportDto.Performers)
			.GetFields();
	}

	public void Compose(IDocumentContainer container)
	{
		container.Page(
			page =>
			{
				page.Header().Padding(GenerateReportHelper.DefaultPaddingSize)
					.Text($"Отчет по задаче {_taskReportDto.Id}").AlignCenter().FontSize(GenerateReportHelper.HeaderFontSize);

				page.Content()
					.Column(
						column =>
						{
							GenerateTaskSection(column);

							GenerateProjectManagerSection(column);

							GenerateManagerSection(column);

							GeneratePerformersSection(column);

							GenerateObserversSection(column);
						}
					);
			}
		);
	}

	private void GenerateObserversSection(ColumnDescriptor column)
	{
		column
			.Item()
			.Element(
				container =>
					GenerateReportHelper.GenerateHeaderSection(container, "Наблюдатели")
				);

		if (_observers.Count > 0)
		{
			column
				.Item()
				.Element(
					container =>
						GenerateReportHelper.GenerateTableTypeSecond(container, _observers)
					);
		}
		else
		{
			GenerateReportHelper
				.GenerateNotFoundMessage(
					column.Item(), "Информация о Наблюдателях отстуствует");
		}
	}

	private void GeneratePerformersSection(ColumnDescriptor column)
	{
		column
			.Item()
			.Element(
				container =>
					GenerateReportHelper.GenerateHeaderSection(container, "Исполнители")
				);

		if (_performers.Count > 0)
		{
			column
				.Item()
				.Element(
					container =>
						GenerateReportHelper.GenerateTableTypeSecond(container, _performers)
					);
		}
		else
		{
			GenerateReportHelper
				.GenerateNotFoundMessage(
					column.Item(), "Информация о Исполнителях отстуствует");
		}
	}

	private void GenerateManagerSection(ColumnDescriptor column)
	{
		column
			.Item()
			.Element(
				container =>
					GenerateReportHelper.GenerateHeaderSection(container, "Информация о Менеджере")
				);

		if (managerProperties.Count > 0)
		{
			column
				.Item()
				.Element(
					container =>
						GenerateReportHelper.GenerateTableTypeFirst(container, managerProperties)
					);
		}
		else
		{
			GenerateReportHelper
				.GenerateNotFoundMessage(
					column.Item(), "Информация о менеджере отсутствует");
		}
	}

	private void GenerateProjectManagerSection(ColumnDescriptor column)
	{
		column
			.Item()
			.Element(
				container =>
					GenerateReportHelper.GenerateHeaderSection(container, "Информация о Менеджере проекта")
				);

		column
			.Item()
			.Element(
				container =>
					GenerateReportHelper.GenerateTableTypeFirst(container, projectManagerProperties)
				);
	}

	private void GenerateTaskSection(ColumnDescriptor column)
	{
		column
			.Item()
			.Element(
				container =>
					GenerateReportHelper.GenerateHeaderSection(container, "Информация о задаче")
				);

		column
			.Item()
			.Element(
				container =>
					GenerateReportHelper.GenerateTableTypeFirst(container, taskProperties)
				);
	}
}
