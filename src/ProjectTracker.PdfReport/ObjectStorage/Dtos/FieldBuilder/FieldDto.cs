namespace ProjectTracker.PdfReport.ObjectStorage.Dtos.FieldBuilder;

public class FieldDto<T>
{
	public required string Name { get; set; }
	public required Func<T, object?> Selector { get; set; }
	public required Type FieldType { get; set; }
	public Func<object?, string?> AfterProcess { get; set; } = x => x?.ToString();
}
