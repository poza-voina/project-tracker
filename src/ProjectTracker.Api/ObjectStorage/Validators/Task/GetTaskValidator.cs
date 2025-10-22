using FluentValidation;
using ProjectTracker.Contracts.ViewModels.Task;

namespace ProjectTracker.Api.ObjectStorage.Validators.Task;

public class GetTaskValidator : AbstractValidator<GetTaskRequest>
{
	public GetTaskValidator()
	{
		RuleFor(x => x.Id)
			.GreaterThan(0)
			.WithMessage("Идентификатор задачи должен быть больше 0");
	}
}
