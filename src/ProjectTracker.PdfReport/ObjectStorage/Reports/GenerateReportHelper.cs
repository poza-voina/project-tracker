using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace ProjectTracker.PdfReport.ObjectStorage.Reports;

public static class GenerateReportHelper
{
	public const int HeaderSectionFontSize = 16;
	public const int HeaderFontSize = 24;
	public const int BorderSize = 1;
	public const string CellEmptySpace = "-";
	public const int DefaultPaddingSize = 10;
	public const int DefaultCellPadding = 5;

	public static void GenerateUnitContainer(this IContainer container, Action<IContainer> buildContent)
	{
		container.Element(container =>
		{
			container.ShowEntire();

			container.Column(column =>
			{
				column.Item().Element(innerContainer =>
				{
					buildContent(innerContainer);
				});
			});
		});
	}

	public static void GenerateNotFoundMessage(this IContainer container, string text)
	{
		container
			.Padding(DefaultPaddingSize)
			.Text(text)
			.FontColor(Color.FromHex("#808080"));
	}

	public static void GenerateHeaderSection(this IContainer container, string text)
	{
		container
			.Text(text)
			.Bold()
			.AlignCenter()
			.FontSize(HeaderSectionFontSize);
	}

	public static void GenerateTableTypeFirst(this IContainer container, Dictionary<string, string?> properties)
	{
		container.Padding(DefaultPaddingSize).Table(
			table =>
			{
				table.ColumnsDefinition(
					columnDefinition =>
					{
						columnDefinition.RelativeColumn();
						columnDefinition.RelativeColumn();
					}
				);

				foreach (var item in properties.Keys)
				{
					table
						.Cell()
						.BorderBottom(BorderSize)
						.PaddingVertical(DefaultCellPadding)
						.Text($"{item}");

					var value = properties[item];
					table
						.Cell()
						.BorderBottom(BorderSize)
						.PaddingVertical(DefaultCellPadding)
						.Text(string.IsNullOrWhiteSpace(value) ? CellEmptySpace : value);
				}
			}
		);
	}

	public static void GenerateTableTypeSecond(this IContainer container, Dictionary<string, List<string?>> properties)
	{
		container.Padding(DefaultPaddingSize).Table(
			table =>
			{
				table.Header(
						header =>
						{
							foreach (var property in properties.Keys)
							{
								header
									.Cell()
									.Border(BorderSize)
									.Text(property)
									.AlignCenter();
							}
						}
					);

				table.ColumnsDefinition(
					columnDefinition =>
					{
						for (int i = 0; i < properties.Keys.Count; i++)
						{
							columnDefinition.RelativeColumn();
						}
					}
				);

				int rowCount = properties.Values.Max(x => x.Count);
				for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
				{
					foreach (var column in properties)
					{
						var value = rowIndex < column.Value.Count ? column.Value[rowIndex] : CellEmptySpace;
						table
							.Cell()
							.Border(BorderSize)
							.Text(value ?? CellEmptySpace)
							.AlignCenter();
					}
				}
			}
		);
	}
}
