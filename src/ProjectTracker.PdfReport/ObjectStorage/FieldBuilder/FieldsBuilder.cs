using Minio.DataModel;
using ProjectTracker.PdfReport.ObjectStorage.Dtos.FieldBuilder;
using System.Reflection;
using System.Reflection.Emit;

namespace ProjectTracker.PdfReport.ObjectStorage.FieldBuilder;

public class FieldsBuilder<T> : BaseFieldsBuilder<T>
{
	public Dictionary<string, string?> BuildWithPrimitiveFields(T? obj)
	{
		Dictionary<string, string?> values;
		if (obj is { })
		{
			values = Build(obj);
		}
		else
		{
			values = new Dictionary<string, string?>();
		}

		var properties = obj?.GetType().GetProperties();

		if (properties is null)
		{
			_status = FieldBuilderStatus.All;

			return values;
		}

		foreach (var property in properties)
		{
			ProcessProperty(obj, property, values);
		}

		_status = FieldBuilderStatus.All;
		return values;
	}

	public Dictionary<string, string?> Build(T data)
	{
		Dictionary<string, string?> values = new();

		foreach (var field in _fields)
		{
			var rawValue = field.Selector(data);

			if (rawValue is null)
			{
				values[field.Name] = null;
			}
			else
			{
				var castedValue = Convert.ChangeType(field.Selector(data), field.FieldType);

				values[field.Name] = field.AfterProcess(castedValue);
			}
		}

		return values;
	}

	private void ProcessProperty(T? obj, PropertyInfo? property, Dictionary<string, string?> values)
	{
		if (property is null)
		{
			return;
		}

		if (FieldsBuilderHelper.IsPrimitiveType(property) &&
			FieldsBuilderHelper.GetKeyOrDefault(property) is string key)
		{
			values[key] = property.GetValue(obj)?.ToString();
		}
	}
}