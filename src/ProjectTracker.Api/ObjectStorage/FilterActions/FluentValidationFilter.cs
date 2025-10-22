using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ProjectTracker.Api.ObjectStorage.FilterActions;

public class FluentValidationFilter(IServiceProvider services) : IAsyncActionFilter
{
	public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
	{

		if (context.ActionDescriptor is not ControllerActionDescriptor controllerActionDescriptor)
		{
			await next();
			return;
		}

		var methodInfo = controllerActionDescriptor.MethodInfo;
		var parameters = methodInfo.GetParameters();

		foreach (var item in parameters)
		{
			var tempValidatorType = typeof(IValidator<>).MakeGenericType(item.ParameterType);
			var validator = services.GetService(tempValidatorType) as IValidator;

			if (validator is null)
			{
				continue;
			}
			else if (item.Name is { } && context.ActionArguments.TryGetValue(item.Name, out var value))
			{
				var result = await validator.ValidateAsync(new ValidationContext<object?>(value));

				if (result.Errors.Any())
				{
					throw new ValidationException(result.Errors);
				}
			}
			else
			{
				throw new ValidationException([new ValidationFailure("request", "Запрос не смог десериализоваться. Пропущены обязательные ключи")]);
			}
		}

		await next();
	}
}
