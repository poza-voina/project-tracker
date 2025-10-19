using ProjectTracker.Abstractions.Exceptions;
using ProjectTracker.Contracts.Events.Reports;
using ProjectTracker.Core.ObjectStorage.Interfaces;
using System.Collections.Concurrent;

namespace ProjectTracker.Core.ObjectStorage;

//TODO сделать нормальную ошибку тут сложно
public class ReportEventAwaiter : IReportEventAwaiter
{
	private readonly ConcurrentDictionary<Guid, TaskCompletionSource<string?>> _waiters = new();

	public ReportEventAwaiter AddReportId(Guid reportId)
	{
		_waiters[reportId] = new TaskCompletionSource<string?>(TaskCreationOptions.RunContinuationsAsynchronously);
		
		return this;
	}

	public async Task<string?> WaitEvent(Guid reportId, TimeSpan? timeout = null)
	{
		if (!_waiters.TryGetValue(reportId, out var tcs))
		{
			throw new UnprocessableException($"reportId = {reportId} не зарегистрирован для ожидания");
		}

		if (timeout.HasValue)
		{
			using var cts = new CancellationTokenSource(timeout.Value);
			var completedTask = await Task.WhenAny(tcs.Task, Task.Delay(Timeout.Infinite, cts.Token));
			if (completedTask != tcs.Task)
				throw new TimeoutException($"Не дождались события для отчёта {reportId}");
		}
		else
		{
			await tcs.Task;
		}

		return tcs.Task.Result;
	}

	public void ProcessResultEvent(ReportResultEvent @event)
	{
		if (_waiters.TryRemove(@event.ReportId, out var tcs))
		{
			tcs.TrySetResult(@event.Url);
		}
		else
		{
			throw new UnprocessableException($"reportId = {@event.ReportId} не найден");
		}
	}

	public void ProcessErrorEvent(ReportErrorEvent @event)
	{
		if(_waiters.TryRemove(@event.ReportId, out var tcs))
		{
			tcs.TrySetResult(null);
		}
		else
		{
			throw new UnprocessableException($"reportId = {@event.ReportId} не найден");
		}
	}

	public bool IsExists(Guid reportId)
	{
		return _waiters.ContainsKey(reportId);
	}
}
