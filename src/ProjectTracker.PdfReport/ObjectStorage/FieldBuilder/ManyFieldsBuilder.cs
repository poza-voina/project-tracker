using ProjectTracker.PdfReport.ObjectStorage.Dtos.FieldBuilder;
using System.Reflection;

namespace ProjectTracker.PdfReport.ObjectStorage.FieldBuilder;

public class ManyFieldsBuilder<T> : BaseFieldsBuilder<T>
{
	public Dictionary<string, List<string?>> BuildWithPrimitiveFields(IEnumerable<T> objs)
	{
		Dictionary<string, List<string?>> values = Build(objs);

		if (objs is null || objs.Count() == 0)
		{
			_status = FieldBuilderStatus.All;
			return values;
		}

		var properties = typeof(T).GetProperties();

		foreach (var obj in objs)
		{
			foreach (var property in properties)
			{
				ProcessProperty(obj, property, values);
			}
		}

		_status = FieldBuilderStatus.All;
		return values;
	}

	public Dictionary<string, List<string?>> Build(IEnumerable<T> data)
	{
		Dictionary<string, List<string?>> values = new();

		foreach (var field in _fields)
		{
			var key = field.Name;

			values.Add(key, new List<string?>());

			foreach (var item in data)
			{
				var targetType = Nullable.GetUnderlyingType(field.FieldType) ?? field.FieldType;

				var rawValue = field.Selector(item);

				if (rawValue is null)
				{
					values[key].Add(null);
				}
				else
				{
					var castedValue = Convert.ChangeType(field.Selector(item), targetType);

					var value = field.AfterProcess(castedValue);

					values[key].Add(value);
				}
			}
		}

		return values;
	}

	private void ProcessProperty(T? obj, PropertyInfo? property, Dictionary<string, List<string?>> values)
	{
		if (obj is null || property is null)
		{
			return;
		}

		if (FieldsBuilderHelper.IsPrimitiveType(property) &&
			FieldsBuilderHelper.GetKeyOrDefault(property) is string key)
		{
			if (values.TryGetValue(key, out var temp))
			{
				temp.Add(property.GetValue(obj)?.ToString());
			}
			else
			{
				var data = new List<string?>
				{
					property.GetValue(obj)?.ToString()
				};

				values[key] = data;
			}
		}
	}
}
