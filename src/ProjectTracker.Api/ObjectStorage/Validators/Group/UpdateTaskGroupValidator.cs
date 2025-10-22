using FluentValidation;
using ProjectTracker.Contracts.ViewModels.TaskGroup;

namespace ProjectTracker.Api.ObjectStorage.Validators.Group;

public class UpdateTaskGroupValidator : AbstractValidator<UpdateTaskGroupRequest>
{
	public UpdateTaskGroupValidator()
	{
		RuleFor(x => x.Id)
			.GreaterThan(0)
			.WithMessage("Идентификатор группы должен быть больше 0");

		RuleFor(x => x.Name)
			.NotEmpty()
			.WithMessage("Название группы не может быть пустым");
	}
}
