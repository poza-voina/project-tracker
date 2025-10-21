using FluentValidation;
using ProjectTracker.Contracts.ViewModels.Task;

namespace ProjectTracker.Api.ObjectStorage.Validators.Task;

public class ChangeTaskStatusValidator : AbstractValidator<ChangeTaskStatusRequest>
{
	public ChangeTaskStatusValidator()
	{
		RuleFor(x => x.TaskId)
			.GreaterThan(0)
			.WithMessage("Идентификатор задачи должен быть больше 0");

		RuleFor(x => x.TaskFlowNodeId)
			.GreaterThan(0)
			.WithMessage("Идентификатор узла потока задач должен быть больше 0");

		RuleFor(x => x.TaskVersion)
			.GreaterThan(0u)
			.WithMessage("Версия задачи должна быть больше 0");
	}
}
