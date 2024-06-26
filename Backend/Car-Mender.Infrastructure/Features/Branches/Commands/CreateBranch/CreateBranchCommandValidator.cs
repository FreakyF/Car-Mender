using Car_Mender.Domain.Features.Branches.Entities;
using Car_Mender.Infrastructure.Validators;
using FluentValidation;

namespace Car_Mender.Infrastructure.Features.Branches.Commands.CreateBranch;

public class CreateBranchCommandValidator : AbstractValidator<CreateBranchCommand>
{
	public CreateBranchCommandValidator()
	{
		RuleFor(c => c.Email)
			.NotEmpty().WithMessage($"{nameof(Branch.Email)} is required")
			.EmailAddress().WithMessage($"Invalid {nameof(Branch.Email).ToLowerInvariant()} format");

		RuleFor(b => b.Address)
			.NotNull().WithMessage($"{nameof(Branch.Address)} is required")
			.SetValidator(new AddressValidator());

		RuleFor(c => c.Name)
			.NotEmpty().WithMessage($"{nameof(Branch.Name)} is required")
			.Length(3, 255).WithMessage($"{nameof(Branch.Name)} must be between 3 and 255 characters");

		RuleFor(c => c.Phone)
			.NotEmpty().WithMessage($"{nameof(Branch.Phone)} is required");
	}
}