using FluentValidation;
using ProjectTracker.Contracts.ViewModels.TaskGroup;

namespace ProjectTracker.Api.ObjectStorage.Validators.Group;

public class GetGroupValidator : AbstractValidator<GetGroupRequest>
{
	public GetGroupValidator()
	{
		RuleFor(x => x.Id)
			.GreaterThan(0)
			.WithMessage("Идентификатор не может быть меньше 0");
	}
}