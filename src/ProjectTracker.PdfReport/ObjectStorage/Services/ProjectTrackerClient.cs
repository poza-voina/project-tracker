using ProjectTracker.Contracts.ViewModels.Shared.Result;
using ProjectTracker.Contracts.ViewModels.Task;
using ProjectTracker.Contracts.ViewModels.TaskGroup;
using ProjectTracker.PdfReport.ObjectStorage.Data;
using ProjectTracker.PdfReport.ObjectStorage.Services.Interfaces;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ProjectTracker.PdfReport.ObjectStorage.Services;

public class ProjectTrackerClient(
	ILogger<ProjectTrackerClient> logger,
	IHttpClientFactory httpClientFactory,
	ProjectTrackerClientConfiguration projectTrackerClientConfiguration) : IProjectTrackerClient
{
	public async Task<MbResult<TaskReportInformationResponse>?> GetTaskOrDefaultAsync(
		long taskId,
		CancellationToken cancellationToken = default)
	{

		MbResult<TaskReportInformationResponse> task;
		try
		{
			task = await GetTaskAsync(taskId, cancellationToken);
		}
		catch (Exception)
		{
			return null;
		}

		return task;
	}

	public async Task<MbResult<TaskGroupInformationResponse>?> GetTaskGroupOrDefaultAsync(
		long taskGroupId,
		CancellationToken cancellationToken = default)
	{

		MbResult<TaskGroupInformationResponse> task;
		try
		{
			task = await GetTaskGroupAsync(taskGroupId, cancellationToken);
		}
		catch (Exception)
		{
			return null;
		}

		return task;
	}

	private async Task<MbResult<TaskGroupInformationResponse>> GetTaskGroupAsync(long taskGroupId, CancellationToken cancellationToken)
	{
		var httpClient = httpClientFactory.CreateClient();
		var uriBuilder = new UriBuilder(httpClient.BaseAddress ?? new Uri(projectTrackerClientConfiguration.DefaultUri))
		{
			Path = $"{projectTrackerClientConfiguration.TaskGroupsBasePath}/{taskGroupId}",
		};

		logger.LogInformation("Отправлен запрос на url = {}", uriBuilder.Uri.ToString());

		var response = await httpClient.GetAsync(uriBuilder.Uri, cancellationToken);
		response.EnsureSuccessStatusCode();

		var options = new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true,
			Converters = { new JsonStringEnumConverter() }
		};

		var result = await response.Content.ReadFromJsonAsync<MbResult<TaskGroupInformationResponse>>(options, cancellationToken);

		ArgumentNullException.ThrowIfNull(result);

		return result;
	}

	private async Task<MbResult<TaskReportInformationResponse>> GetTaskAsync(long taskId, CancellationToken cancellationToken = default)
	{
		var httpClient = httpClientFactory.CreateClient();
		var uriBuilder = new UriBuilder(httpClient.BaseAddress ?? new Uri(projectTrackerClientConfiguration.DefaultUri))
		{
			Path = $"{projectTrackerClientConfiguration.TasksBasePath}/{taskId}",
		};

		logger.LogInformation("Отправлен запрос на url = {}", uriBuilder.Uri.ToString());

		var response = await httpClient.GetAsync(uriBuilder.Uri, cancellationToken);
		response.EnsureSuccessStatusCode();

		var options = new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true,
			Converters = { new JsonStringEnumConverter() }
		};

		var result = await response.Content.ReadFromJsonAsync<MbResult<TaskReportInformationResponse>>(options, cancellationToken);

		ArgumentNullException.ThrowIfNull(result);

		logger.LogWarning("Не удалось десериализовать объект", uriBuilder.Uri.ToString());

		return result;
	}
}
