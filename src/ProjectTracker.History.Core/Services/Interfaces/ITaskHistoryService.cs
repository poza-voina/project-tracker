
using ProjectTracker.Contracts.Events.HistoryEvents;
using ProjectTracker.History.Contracts.ViewModels.TaskHistory;

namespace ProjectTracker.History.Core.Services.Interfaces;

public interface ITaskHistoryService
{
	Task<IEnumerable<GetTasksHistoryResponse>> GetAllAsync(GetTasksHistoryRequest request);
	Task ProcessHistoryEventAsync(HistoryEvent message);
}
