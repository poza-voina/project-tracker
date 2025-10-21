using FluentValidation;
using ProjectTracker.Contracts.ViewModels.Shared.Pagination;

namespace ProjectTracker.Api.ObjectStorage.Validators.Shared;

public class PaginationRequestValidator : AbstractValidator<PaginationRequest>
{
	public PaginationRequestValidator()
	{
		RuleFor(x => x.PageNumber)
			.GreaterThan(0)
			.WithMessage("Номер страницы должен быть больше 0");

		RuleFor(x => x.PageSize)
			.GreaterThan(0)
			.WithMessage("Размер страницы должен быть больше 0");
	}
}
