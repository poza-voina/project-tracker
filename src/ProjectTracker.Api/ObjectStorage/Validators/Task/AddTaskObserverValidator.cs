using FluentValidation;
using ProjectTracker.Contracts.ViewModels.Task;

namespace ProjectTracker.Api.ObjectStorage.Validators.Task;

public class AddTaskObserverValidator : AbstractValidator<AddTaskObserverRequest>
{
	public AddTaskObserverValidator()
	{
		RuleFor(x => x.EmployeeId)
			.GreaterThan(0)
			.WithMessage("Идентификатор сотрудника должен быть больше 0");

		RuleFor(x => x.TaskId)
			.GreaterThan(0)
			.WithMessage("Идентификатор задачи должен быть больше 0");
	}
}
