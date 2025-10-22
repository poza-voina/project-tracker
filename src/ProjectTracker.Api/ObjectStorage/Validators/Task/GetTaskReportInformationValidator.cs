using FluentValidation;
using ProjectTracker.Contracts.ViewModels.Task;

namespace ProjectTracker.Api.ObjectStorage.Validators.Task;

public class GetTaskReportInformationValidator : AbstractValidator<GetTaskReportRequest>
{
	public GetTaskReportInformationValidator()
	{
		RuleFor(x => x.Id)
			.GreaterThan(0)
			.WithMessage("Идентификатор задачи должен быть больше 0");
	}
}
