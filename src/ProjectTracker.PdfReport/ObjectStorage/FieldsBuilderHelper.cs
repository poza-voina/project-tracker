using MassTransit.MessageData.PropertyProviders;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ProjectTracker.PdfReport.ObjectStorage;

public static class FieldsBuilderHelper
{
	public static string? GetKeyOrDefault(PropertyInfo property)
	{
		return property.GetCustomAttribute<DisplayAttribute>()?.Name;
	}

	public static bool IsPrimitiveType(PropertyInfo property)
	{
		var propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

		return propertyType.IsPrimitive ||
			propertyType.IsEnum ||
			propertyType == typeof(string) ||
			propertyType == typeof(DateTime);
	}
}
