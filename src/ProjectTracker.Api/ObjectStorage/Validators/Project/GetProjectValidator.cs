using FluentValidation;
using ProjectTracker.Contracts.ViewModels.Project;

namespace ProjectTracker.Api.ObjectStorage.Validators.Project;

public class GetProjectValidator : AbstractValidator<GetProjectRequest>
{
	public GetProjectValidator()
	{
		RuleFor(x => x.Id)
			.GreaterThan(0)
			.WithMessage("Идентификатор проекта должен быть больше 0");
	}
}
