using FluentValidation;
using ProjectTracker.Contracts.ViewModels.Report;

namespace ProjectTracker.Api.ObjectStorage.Validators.Report;

public class TaskReportValidator : AbstractValidator<TaskReportRequest>
{
	public TaskReportValidator()
	{
		RuleFor(x => x.TaskId)
			.GreaterThan(0)
			.WithMessage("Идентификатор задачи должен быть больше 0");

		RuleFor(x => x.ExpirySeconds)
			.GreaterThan(0)
			.WithMessage("Время истечения должно быть больше 0");

		RuleFor(x => x.Timeout)
			.Must(x => x is null || x.Value.TotalSeconds > 0)
			.WithMessage("Таймаут должен быть больше 0 или null");
	}
}
