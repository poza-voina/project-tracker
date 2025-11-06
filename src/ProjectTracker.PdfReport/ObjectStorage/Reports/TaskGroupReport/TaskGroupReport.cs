using Mapster;
using ProjectTracker.PdfReport.ObjectStorage.Dtos.TaskGroupReport;
using ProjectTracker.PdfReport.ObjectStorage.FieldBuilder;
using ProjectTracker.PdfReport.ObjectStorage.Reports;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace ProjectTracker.PdfReport.ObjectStorage.Reports.TaskGroupReport;

public class TaskGroupReport : IDocument
{
	private readonly TaskGroupReportDto _taskGroupReportDto;

	private readonly Dictionary<string, string?> _groupProperties;
	private readonly Dictionary<string, List<string?>>? _simpleTaskProperties;

	public TaskGroupReport(TaskGroupReportDto taskGroupReportDto)
	{
		_taskGroupReportDto = taskGroupReportDto;

		_groupProperties = new FieldsBuilder<TaskGroupReportDto>()
			.BuildWithPrimitiveFields(_taskGroupReportDto);

		//NOTE Так делать тоже не очень
		var firstTask = _taskGroupReportDto.Tasks.FirstOrDefault();

		if (firstTask is { })
		{
			//NOTE добавить ссылки через Bookmark
			_simpleTaskProperties = new ManyFieldsBuilder<TaskGroupTaskDto>()
				.Add
				(
					x => x.Id,
					x => x.Name
				)
				.AddWithAfterProcess
				(
					x => x.Deadline,
					x => x?.ToString("dd.MM.yyyy")
				)
				.AddWithAfterProcess
				(
					x => x.CreatedAt,
					x => x.ToString("dd.MM.yyyy")
				)
				.AsManyFieldsBuilder()
				?.Build(_taskGroupReportDto.Tasks);
		}
	}

	public void Compose(IDocumentContainer container)
	{
		container.Page(
			page =>
			{
				page
					.Header()
					.Padding(GenerateReportHelper.DefaultPaddingSize)
					.Text($"Отчет по группе задач: {_taskGroupReportDto.Id}")
					.AlignCenter()
					.FontSize(GenerateReportHelper.HeaderFontSize);

				page.Content()
					.Column(
						column =>
						{
							GenerateGroupSection(column);

							GenerateSimpleTaskSection(column);
							
							GenerateTaskSection(column);
						}
					);
			});
	}

	private void GenerateTaskSection(ColumnDescriptor column)
	{
		foreach (var task in _taskGroupReportDto.Tasks)
		{
			var section = new TaskSection(task);
			
			column
				.Item()
				.Element(section.Compose);
		}
	}

	private void GenerateSimpleTaskSection(ColumnDescriptor column)
	{
		if (_simpleTaskProperties is { })
		{
			column
				.Item()
				.Element(
					container =>
						GenerateReportHelper.GenerateTableTypeSecond(container, _simpleTaskProperties)
					);
		}
	}

	private void GenerateGroupSection(ColumnDescriptor column)
	{
		column
			.Item()
			.Element(
				container =>
					GenerateReportHelper.GenerateTableTypeFirst(container, _groupProperties)
				);
	}
}
