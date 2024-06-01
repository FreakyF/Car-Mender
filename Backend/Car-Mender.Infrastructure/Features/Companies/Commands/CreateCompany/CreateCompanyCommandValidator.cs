using Car_Mender.Domain.Features.Companies.Entities;
using Car_Mender.Infrastructure.Validators;
using FluentValidation;

namespace Car_Mender.Infrastructure.Features.Companies.Commands.CreateCompany;

public class CreateCompanyCommandValidator : AbstractValidator<CreateCompanyCommand>
{
	public CreateCompanyCommandValidator()
	{
		RuleFor(c => c.Email)
			.NotEmpty().WithMessage($"{nameof(Company.Email)} is required")
			.EmailAddress().WithMessage($"Invalid {nameof(Company.Email).ToLowerInvariant()} format");

		RuleFor(c => c.Name)
			.NotEmpty().WithMessage($"{nameof(Company.Name)} is required")
			.Length(3, 255).WithMessage($"{nameof(Company.Name)} must be between 3 and 255 characters");

		RuleFor(c => c.Address)
			.NotNull().WithMessage($"{nameof(Company.Address)} is required")
			.SetValidator(new AddressValidator());

		RuleFor(c => c.Nip)
			.NotEmpty().WithMessage($"{nameof(Company.Nip)} is required");

		RuleFor(c => c.Phone)
			.NotEmpty().WithMessage($"{nameof(Company.Phone)} is required");
	}
}