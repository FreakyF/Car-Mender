using Car_Mender.Domain.Models;
using FluentValidation;

namespace Car_Mender.Infrastructure.Validators;

public class AddressValidator : AbstractValidator<Address>
{
	public AddressValidator()
	{
		RuleFor(a => a.City)
			.NotEmpty().WithMessage($"{nameof(Address.City)} is required");

		RuleFor(a => a.Country)
			.NotEmpty().WithMessage($"{nameof(Address.Country)} is required");

		RuleFor(a => a.Street)
			.NotEmpty().WithMessage($"{nameof(Address.Street)} is required");

		RuleFor(a => a.PostalCode)
			.NotEmpty().WithMessage($"{nameof(Address.PostalCode)} is required");
	}
}