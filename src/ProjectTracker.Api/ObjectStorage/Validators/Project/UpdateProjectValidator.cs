using FluentValidation;
using ProjectTracker.Contracts.ViewModels.Project;

namespace ProjectTracker.Api.ObjectStorage.Validators.Project;

public class UpdateProjectValidator : AbstractValidator<UpdateProjectRequest>
{
	public UpdateProjectValidator()
	{
		RuleFor(x => x.Id)
			.GreaterThan(0)
			.WithMessage("Идентификатор проекта должен быть больше 0");

		RuleFor(x => x.ProjectManagerId)
			.GreaterThan(0)
			.WithMessage("Идентификатор менеджера проекта должен быть больше 0");

		RuleFor(x => x.ManagerId)
			.Must(x => x is null || x > 0)
			.WithMessage("Идентификатор менеджера должен быть больше 0 или null");

		RuleFor(x => x.TaskFlowId)
			.GreaterThan(0)
			.WithMessage("Идентификатор потока задач должен быть больше 0");
	}
}
