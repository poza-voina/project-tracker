using FluentValidation;
using ProjectTracker.Contracts.ViewModels.TaskGroup;

namespace ProjectTracker.Api.ObjectStorage.Validators.Group;

public class GetGroupReportInforamationValidator : AbstractValidator<GetGroupReportInforamationRequest>
{
	public GetGroupReportInforamationValidator()
	{
		RuleFor(x => x.Id)
			.GreaterThan(0)
			.WithMessage("Идентификатор группы должен быть больше 0");
	}
}
