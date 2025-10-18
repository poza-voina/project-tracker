using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ProjectTracker.PdfReport.ObjectStorage;

public class FieldsBuilder
{
	private Dictionary<string, string?> values = new Dictionary<string, string?>();

	public FieldsBuilder AddAllPrimitiveFields<T>(T? obj)
	{
		var properties = obj?.GetType().GetProperties();

		if (properties is null)
		{
			return this;
		}

		foreach (var property in properties)
		{
			ProcessProperty(obj, property);
		}

		return this;
	}

	public FieldsBuilder AddPrimitiveFields<T>(
		T? obj,
		params string[] propertyNames)
	{
		if (obj is null)
		{
			return this;
		}

		foreach (var propName in propertyNames)
		{
			var property = typeof(T).GetProperty(propName);

			ProcessProperty(obj, property);
		}

		return this;
	}

	public Dictionary<string, string?> GetFields()
	{
		return values;
	}

	private void ProcessProperty<T>(T? obj, PropertyInfo? property)
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