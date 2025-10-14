namespace ProjectTracker.Core.ObjectStorage.Interfaces;

public interface IEventDispatcher
{
	Task DispatchAllAsync();
}
