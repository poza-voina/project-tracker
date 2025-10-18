using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Xml.Linq;

namespace ProjectTracker.PdfReport.ObjectStorage;

public class ManyFieldsBuilder
{
	Dictionary<string, List<string?>> values = new();

	public ManyFieldsBuilder AddAllPrimitiveFields<T>(IEnumerable<T> objs)
	{
		if (objs is null || objs.Count() == 0)
		{
			return this;
		}

		var properties = typeof(T).GetProperties();

		foreach (var obj in objs)
		{
			foreach (var property in properties)
			{
				ProcessProperty(obj, property);
			}
		}

		return this;
	}

	public ManyFieldsBuilder AddPrimitiveFields<T>(
		IEnumerable<T> objs,
		params string[] propertyNames)
	{
		var properties = propertyNames
			.Select(typeof(T).GetProperty);

		foreach (var obj in objs)
		{
			foreach(var property in properties)
			{
				ProcessProperty(obj, property);
			}
		}

		return this;
	}

	public Dictionary<string, List<string?>> GetFields()
	{
		return values;
	}

	private void ProcessProperty<T>(T? obj, PropertyInfo? property)
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