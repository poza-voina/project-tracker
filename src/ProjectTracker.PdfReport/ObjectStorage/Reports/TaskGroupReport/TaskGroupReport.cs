using ProjectTracker.PdfReport.ObjectStorage.Dtos.TaskGroupReport;
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

		_groupProperties = new FieldsBuilder()
			.AddAllPrimitiveFields(_taskGroupReportDto)
			.GetFields();

		//NOTE Так делать тоже не очень
		var firstTask = _taskGroupReportDto.Tasks.FirstOrDefault();

		if (firstTask is { })
		{
			//NOTE добавить ссылки через Bookmark
			_simpleTaskProperties = new ManyFieldsBuilder()
				.AddPrimitiveFields(_taskGroupReportDto.Tasks,
					nameof(firstTask.Id),
					nameof(firstTask.CreatedAt),
					nameof(firstTask.Deadline)) //NOTE Не получится получить статус скорее всего нужно делать Билдер с деревом выражений а с ними я работать не умею
				.GetFields();
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
