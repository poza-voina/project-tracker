using ProjectTracker.Core.ObjectStorage.Interfaces;

namespace ProjectTracker.Api.ObjectStorage.Middlewares;

public class EventMiddleware(IEventDispatcher eventDispatcher) : IMiddleware
{
	public async Task InvokeAsync(HttpContext context, RequestDelegate next)
	{
		await next(context);

		await eventDispatcher.DispatchAllAsync();
	}
}
