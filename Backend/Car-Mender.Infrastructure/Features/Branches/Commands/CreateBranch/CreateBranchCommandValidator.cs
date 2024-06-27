using Car_Mender.Domain.Features.Branches.Entities;
using Car_Mender.Infrastructure.Validators;
using FluentValidation;

namespace Car_Mender.Infrastructure.Features.Branches.Commands.CreateBranch;

public class CreateBranchCommandValidator : AbstractValidator<CreateBranchCommand>
{
	public CreateBranchCommandValidator()
	{
		RuleFor(b => b.Email)
			.NotEmpty().WithMessage($"{nameof(Branch.Email)} is required")
			.EmailAddress().WithMessage($"Invalid {nameof(Branch.Email).ToLowerInvariant()} format");

		RuleFor(b => b.Address)
			.NotNull().WithMessage($"{nameof(Branch.Address)} is required")
			.SetValidator(new AddressValidator());

		RuleFor(b => b.Name)
			.NotEmpty().WithMessage($"{nameof(Branch.Name)} is required")
			.Length(3, 255).WithMessage($"{nameof(Branch.Name)} must be between 3 and 255 characters");

		RuleFor(b => b.Phone)
			.NotEmpty().WithMessage($"{nameof(Branch.Phone)} is required");
	}
}