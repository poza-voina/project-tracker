using FluentValidation;
using ProjectTracker.Contracts.ViewModels.Task;

namespace ProjectTracker.Api.ObjectStorage.Validators.Task;

public class GetPaginationTasksValidator : AbstractValidator<GetPaginationTasksRequest>
{
	public GetPaginationTasksValidator()
	{
		RuleFor(x => x.PageNumber)
			.GreaterThan(0)
			.WithMessage("Номер страницы должен быть больше 0");

		RuleFor(x => x.PageSize)
			.GreaterThan(0)
			.WithMessage("Размер страницы должен быть больше 0");

		RuleFor(x => x.ProjectId)
			.Must(x => x is null || x > 0)
			.WithMessage("Идентификатор проекта должен быть больше 0 или null");

		RuleFor(x => x.GroupId)
			.Must(x => x is null || x > 0)
			.WithMessage("Идентификатор группы должен быть больше 0 или null");

		RuleFor(x => x.EmployeeId)
			.Must(x => x is null || x > 0)
			.WithMessage("Идентификатор сотрудника должен быть больше 0 или null");
	}
}
