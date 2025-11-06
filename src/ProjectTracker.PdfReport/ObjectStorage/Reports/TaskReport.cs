using Mapster;
using ProjectTracker.Contracts.ViewModels.Task;
using ProjectTracker.PdfReport.ObjectStorage.Dataproviders;
using ProjectTracker.PdfReport.ObjectStorage.Dtos.TaskReport;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace ProjectTracker.PdfReport.ObjectStorage.Reports;

public class TaskReport : IDocument
{
	TaskReportInputDto _inputData;

	public TaskReport(TaskReportDataProcessor processor, TaskReportDto dto)
	{
		_inputData = processor.Process(dto);
	}

	public void Compose(IDocumentContainer container)
	{
		container.Page(
			page =>
			{
				page.Header().Padding(GenerateReportHelper.DefaultPaddingSize)
					.Text($"Отчет по задаче {_inputData.TaskId}")
					.AlignCenter()
					.FontSize(GenerateReportHelper.HeaderFontSize);

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

		if (_inputData.Observers.Count > 0)
		{
			column
				.Item()
				.Element(
					container =>
						GenerateReportHelper.GenerateTableTypeSecond(container, _inputData.Observers)
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

		if (_inputData.Performers.Count > 0)
		{
			column
				.Item()
				.Element(
					container =>
						GenerateReportHelper.GenerateTableTypeSecond(container, _inputData.Performers)
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

		if (_inputData.ManagerProperties.Count > 0)
		{
			column
				.Item()
				.Element(
					container =>
						GenerateReportHelper.GenerateTableTypeFirst(container, _inputData.ManagerProperties)
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
					GenerateReportHelper.GenerateTableTypeFirst(container, _inputData.ProjectManagerProperties)
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
					GenerateReportHelper.GenerateTableTypeFirst(container, _inputData.TaskProperties)
				);
	}
}
