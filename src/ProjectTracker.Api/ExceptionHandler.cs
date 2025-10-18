using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using ProjectTracker.Abstractions.Exceptions;
using FluentValidation;
using ProjectTracker.Api.ObjectStorage.Data.ViewModels.Shared.Result;
using ProjectTracker.Contracts.ViewModels.Shared.Result;

namespace ProjectTracker.Api;

public class ExceptionHandler : IExceptionHandler
{
	public async ValueTask<bool> TryHandleAsync(
		HttpContext httpContext,
		Exception exception,
		CancellationToken cancellationToken)
	{
		var statusCode = exception switch
		{
			NotFoundException => StatusCodes.Status404NotFound,
			ValidationException => StatusCodes.Status400BadRequest,
			BadRequestException => StatusCodes.Status400BadRequest,
			ConflictException => StatusCodes.Status409Conflict,
			DbUpdateConcurrencyException => StatusCodes.Status409Conflict,
			UnprocessableException => StatusCodes.Status422UnprocessableEntity,
			_ => StatusCodes.Status500InternalServerError
		};

		MbResult<object> result;
		if (exception is ValidationException validationException)
		{
			result = MbResultFactory.WithValidationErrors(validationException.Errors, statusCode);
		}
		else
		{
			result = MbResultFactory.WithOperationError(exception, statusCode);
		}

		httpContext.Response.StatusCode = statusCode;
		await httpContext.Response.WriteAsJsonAsync(result, cancellationToken);

		return true;
	}
}