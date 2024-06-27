using Car_Mender.Domain.Features.Engines.Entities;
using FluentValidation;

namespace Car_Mender.Infrastructure.Features.Engines.Commands.CreateEngine;

public class CreateEngineCommandValidator : AbstractValidator<CreateEngineCommand>
{
	public CreateEngineCommandValidator()
	{
		RuleFor(e => e.EngineCode)
			.NotEmpty().WithMessage($"{nameof(Engine.EngineCode)} cannot be empty.");

		RuleFor(e => e.Displacement)
			.NotEmpty().WithMessage($"{nameof(Engine.Displacement)} cannot be empty.")
			.GreaterThan(0u).WithMessage($"{nameof(Engine.Displacement)} must be greater than zero.");

		RuleFor(e => e.PowerKw)
			.NotEmpty().WithMessage($"{nameof(Engine.PowerKw)} cannot be empty.")
			.GreaterThan(0u).WithMessage($"{nameof(Engine.PowerKw)} must be greater than zero.");

		RuleFor(e => e.TorqueNm)
			.NotEmpty().WithMessage($"{nameof(Engine.TorqueNm)} cannot be empty.")
			.GreaterThan(0u).WithMessage($"{nameof(Engine.TorqueNm)} must be greater than zero.");

		RuleFor(e => e.FuelType)
			.NotEmpty().WithMessage($"{nameof(Engine.FuelType)} cannot be empty.")
			.IsInEnum().WithMessage($"{nameof(Engine.FuelType)} must be a valid fuel type.");
	}
}