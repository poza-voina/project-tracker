using FluentValidation;
using ProjectTracker.Contracts.ViewModels.Task;

namespace ProjectTracker.Api.ObjectStorage.Validators.Task;

public class UpdateTaskValidator : AbstractValidator<UpdateTaskRequest>
{
	public UpdateTaskValidator()
	{
		RuleFor(x => x.Id)
			.GreaterThan(0)
			.WithMessage("Идентификатор задачи должен быть больше 0");

		RuleFor(x => x.Name)
			.NotEmpty()
			.WithMessage("Название задачи не может быть пустым");

		RuleFor(x => x.ProjectId)
			.GreaterThan(0)
			.WithMessage("Идентификатор проекта должен быть больше 0");

		RuleFor(x => x.GroupId)
			.Must(x => x is null || x > 0)
			.WithMessage("Идентификатор группы должен быть больше 0 или null");

		RuleFor(x => x.Deadline)
			.Must(x => x is null || x.Value > DateTime.Now)
			.WithMessage("Срок выполнения должен быть в будущем или null");
	}
}
