using FluentValidation;
using ProjectTracker.Contracts.ViewModels.TaskGroup;

namespace ProjectTracker.Api.ObjectStorage.Validators.Group;

public class DeleteGroupValidator : AbstractValidator<DeleteGroupRequest>
{
	public DeleteGroupValidator()
	{
		RuleFor(x => x.Id)
			.GreaterThan(0)
			.WithMessage("Идентификатор группы должен быть больше 0");
	}
}
