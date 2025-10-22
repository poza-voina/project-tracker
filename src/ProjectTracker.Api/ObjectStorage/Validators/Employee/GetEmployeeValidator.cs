using FluentValidation;
using ProjectTracker.Contracts.ViewModels.Employee;

namespace ProjectTracker.Api.ObjectStorage.Validators.Employee;

public class GetEmployeeValidator : AbstractValidator<GetEmployeeRequest>
{
	public GetEmployeeValidator()
	{
		RuleFor(x => x.Id)
			.GreaterThan(0)
			.WithMessage("Идентификатор сотрудника должен быть больше 0");
	}
}

