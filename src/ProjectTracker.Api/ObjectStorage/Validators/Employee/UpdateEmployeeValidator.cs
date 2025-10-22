using FluentValidation;
using ProjectTracker.Contracts.ViewModels.Employee;

namespace ProjectTracker.Api.ObjectStorage.Validators.Employee;

public class UpdateEmployeeValidator : AbstractValidator<UpdateEmployeeRequest>
{
	public UpdateEmployeeValidator()
	{
		RuleFor(x => x.Id)
			.GreaterThan(0)
			.WithMessage("Идентификатор не может быть меньше 0");

		RuleFor(x => x.LastName)
			.NotEmpty()
			.WithMessage("Фамилия не может быть пустой");

		RuleFor(x => x.FirstName)
			.NotEmpty()
			.WithMessage("Имя не может быть пустым");

		RuleFor(x => x.Username)
			.NotEmpty()
			.WithMessage("Имя пользователя не может быть пустым");

		RuleFor(x => x.Role)
			.IsInEnum()
			.WithMessage("Роль сотрудника должна быть валидной");
	}
}
