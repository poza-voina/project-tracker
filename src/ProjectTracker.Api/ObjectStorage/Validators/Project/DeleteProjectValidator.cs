using FluentValidation;
using ProjectTracker.Contracts.ViewModels.Project;

namespace ProjectTracker.Api.ObjectStorage.Validators.Project;

public class DeleteProjectValidator : AbstractValidator<DeleteProjectRequest>
{
	public DeleteProjectValidator()
	{
		RuleFor(x => x.Id)
			.GreaterThan(0)
			.WithMessage("Идентификатор проекта должен быть больше 0");
	}
}
