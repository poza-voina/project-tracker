using FluentValidation;
using ProjectTracker.Contracts.ViewModels.Employee;

namespace ProjectTracker.Api.ObjectStorage.Validators.Employee;

public class DeleteEmployeeValidator : AbstractValidator<DeleteEmployeeRequest>
{
	public DeleteEmployeeValidator()
	{
		RuleFor(x => x.Id)
			.GreaterThan(0)
			.WithMessage("Идентификатор не может быть меньше 0");
	}
}
