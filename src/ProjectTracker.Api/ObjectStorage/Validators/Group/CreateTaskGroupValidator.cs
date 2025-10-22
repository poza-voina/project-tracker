using FluentValidation;
using ProjectTracker.Contracts.ViewModels.TaskGroup;

namespace ProjectTracker.Api.ObjectStorage.Validators.Group;

public class CreateTaskGroupValidator : AbstractValidator<CreateTaskGroupRequest>
{
	public CreateTaskGroupValidator()
	{
		RuleFor(x => x.Name)
			.NotEmpty()
			.WithMessage("Название группы не может быть пустым");
	}
}
