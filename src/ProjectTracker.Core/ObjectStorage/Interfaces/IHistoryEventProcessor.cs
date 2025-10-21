using ProjectTracker.Contracts.Events.HistoryEvents;
using ProjectTracker.Core.ObjectStorage.Dtos.History;
using ProjectTracker.Infrastructure.Models;

namespace ProjectTracker.Core.ObjectStorage.Interfaces;

public interface IHistoryEventProcessor
{
	HistoryEvent? CreateHistoryEventOrDefault<T>(
		T? old,
		T? current,
		HistoryEventMeta meta) where T : IHistoryDto;
}
