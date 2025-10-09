﻿using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using ProjectTracker.Contracts.ViewModels.Shared.Result;

namespace ProjectTracker.Api.ObjectStorage.Data.ViewModels.Shared.Result;

public static class MbResultFactory
{
	public static MbResult<object> WithValidationErrors(IEnumerable<ValidationFailure> errors, int statusCode)
	{
		return new MbResult<object>
		{
			ValidationErrors = errors.Select(
			x => new ValidationError
			{
				Field = x.PropertyName,
				Message = x.ErrorMessage
			}),
			StatusCode = statusCode
		};
	}

	public static MbResult<object> WithOperationError(Exception exception, int statusCode)
	{
		return new MbResult<object>
		{
			OperationError = new OperationError
			{
				Message = exception.Message,
				StackTrace = exception.StackTrace,
				ExceptionType = exception.GetType().FullName
			},
			StatusCode = statusCode
		};
	}

	public static MbResult<object> WithOperationError(string message, int statusCode)
	{
		return new MbResult<object>
		{
			OperationError = new OperationError
			{
				Message = message
			},
			StatusCode = statusCode
		};
	}

	public static MbResult<T> WithSuccess<T>(T? data)
	{
		return new MbResult<T>
		{
			Result = data,
			StatusCode = StatusCodes.Status200OK
		};
	}
}
