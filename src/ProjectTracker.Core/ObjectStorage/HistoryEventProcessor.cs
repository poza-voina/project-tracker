using Microsoft.Extensions.Logging;
using ProjectTracker.Contracts.Events.HistoryEvents;
using ProjectTracker.Core.ObjectStorage.Dtos.History;
using ProjectTracker.Core.ObjectStorage.Interfaces;
using ProjectTracker.Infrastructure.Models;
using System.Reflection;

namespace ProjectTracker.Core.ObjectStorage;

public class HistoryEventProcessor(ILogger<HistoryEventProcessor> logger) : IHistoryEventProcessor
{
	private readonly List<PropertyChange> _changes = new();

	public HistoryEvent? CreateHistoryEventOrDefault<T>(
		T? old,
		T? current,
		HistoryEventMeta meta) where T : IHistoryDto
	{
		if (old is null && current is null)
		{
			return null;
		}

		try
		{
			return ProcessProperties(old, current, meta);
		} catch (Exception ex)
		{
			logger.LogError($"Ошибка создания эвента истории: {ex}");
		}

		return null;
		
	}

	private HistoryEvent ProcessProperties<T>(T? old, T? current, HistoryEventMeta meta) where T : IHistoryDto
	{
		var properties = typeof(T).GetProperties();

		foreach (var property in properties)
		{
			if (IsPrimitiveType(property))
			{
				ProcessPrimitiveType(property, old, current);
			}
			//NOTE с коллекциями что-то непонятно, а точнее с типами тут как-то не очень получилось
			else if (property.PropertyType.IsGenericType &&
				property.PropertyType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
			{
				ProcessLongCollection(property, old, current);
			}
		}

		return new HistoryEvent
		{
			Meta = meta,
			Changes = _changes
		};
	}

	private void ProcessLongCollection<T>(PropertyInfo property, T? old, T? current) where T : IHistoryDto
	{
		var oldCollection = (property.GetValue(old) as IEnumerable<long>)?.ToHashSet();
		var newCollection = (property.GetValue(current) as IEnumerable<long>)?.ToHashSet();

		if (oldCollection is null && newCollection is null)
		{
		}
		else if (oldCollection is null || newCollection is null)
		{

		}
		else if (!oldCollection.SequenceEqual(newCollection))
		{
			_changes.Add(new PropertyChange
			{
				Property = property.Name,
				OldValue = string.Join(", ", oldCollection),
				NewValue = string.Join(", ", newCollection)
			});
		}
	}

	private void ProcessPrimitiveType<T>(
		PropertyInfo property,
		T? old,
		T? current) where T: IHistoryDto
	{
		var newValue = property.GetValue(current);

		if (old is null)
		{
			_changes.Add(new PropertyChange
			{
				Property = property.Name,
				OldValue = null,
				NewValue = newValue?.ToString()
			});

			return;
		}

		var oldValue = property.GetValue(old);
		if (current is null)
		{
			_changes.Add(new PropertyChange
			{
				Property = property.Name,
				OldValue = newValue?.ToString(),
				NewValue = null
			});

			return;
		}

		if (oldValue != newValue)
		{
			_changes.Add(new PropertyChange
			{
				Property = property.Name,
				OldValue = oldValue?.ToString(),
				NewValue = newValue?.ToString()
			});
		}
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
