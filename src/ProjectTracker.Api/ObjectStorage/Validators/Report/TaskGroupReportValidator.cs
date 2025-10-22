using FluentValidation;
using ProjectTracker.Contracts.ViewModels.Report;

namespace ProjectTracker.Api.ObjectStorage.Validators.Report;

public class TaskGroupReportValidator : AbstractValidator<TaskGroupReportRequest>
{
	public TaskGroupReportValidator()
	{
		RuleFor(x => x.GroupId)
			.GreaterThan(0)
			.WithMessage("Идентификатор группы должен быть больше 0");

		RuleFor(x => x.ExpirySeconds)
			.NotEmpty()
			.WithMessage("Время истечения ссылки не может быть пустым")
			.GreaterThan(0)
			.WithMessage("Время истечения ссылки должно быть больше 0");

		RuleFor(x => x.Timeout)
			.Must(x => x is null || x.Value.TotalSeconds > 0)
			.WithMessage("Таймаут должен быть больше 0 или null");
	}
}
