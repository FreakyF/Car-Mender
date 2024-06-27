using Car_Mender.Domain.Features.Workers.Entities;
using FluentValidation;

namespace Car_Mender.Infrastructure.Features.Workers.Commands.CreateWorker;

public class CreateWorkerCommandValidator : AbstractValidator<CreateWorkerCommand>
{
	public CreateWorkerCommandValidator()
	{
		RuleFor(w => w.Email)
			.NotEmpty().WithMessage($"{nameof(Worker.Email)} is required")
			.EmailAddress().WithMessage($"Invalid {nameof(Worker.Email).ToLowerInvariant()} format");

		RuleFor(w => w.Password)
			.NotEmpty().WithMessage($"{nameof(Worker.Password)} is required")
			.MinimumLength(8).WithMessage($"{nameof(Worker.Password)} must be at least 8 characters long")
			.MaximumLength(100).WithMessage($"{nameof(Worker.Password)} must not exceed 100 characters")
			.Matches(@"[A-Z]").WithMessage($"{nameof(Worker.Password)} must contain at least one uppercase letter")
			.Matches(@"[a-z]").WithMessage($"{nameof(Worker.Password)} must contain at least one lowercase letter")
			.Matches(@"\d").WithMessage($"{nameof(Worker.Password)} must contain at least one digit")
			.Matches(@"[\W_]+").WithMessage($"{nameof(Worker.Password)} must contain at least one special character");

		RuleFor(w => w.FirstName)
			.NotEmpty().WithMessage($"{nameof(Worker.FirstName)} is required");

		RuleFor(w => w.LastName)
			.NotEmpty().WithMessage($"{nameof(Worker.LastName)} is required");

		RuleFor(w => w.Phone)
			.NotEmpty().WithMessage($"{nameof(Worker.Phone)} is required");
	}
}