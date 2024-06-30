using Car_Mender.Domain.Features.Vehicles.Entities;
using FluentValidation;

namespace Car_Mender.Infrastructure.Features.Vehicles.Commands.CreateVehicle;

public class CreateVehicleCommandValidator : AbstractValidator<CreateVehicleCommand>
{
	public CreateVehicleCommandValidator()
	{
		RuleFor(v => v.Generation)
			.NotEmpty().WithMessage($"{nameof(Vehicle.Generation)} cannot be empty.");

		RuleFor(v => v.Make)
			.NotEmpty().WithMessage($"{nameof(Vehicle.Make)} cannot be empty.");

		RuleFor(v => v.Model)
			.NotEmpty().WithMessage($"{nameof(Vehicle.Model)} cannot be empty.");

		RuleFor(v => v.Vin)
			.Must((vehicle, vin) => ValidateVinLength(vehicle.Year, vin))
			.WithMessage($"{nameof(Vehicle.Model)} VIN length is not valid based on the year.")
			.When(v => !string.IsNullOrEmpty(v.Vin));

		RuleFor(v => v.Year)
			.NotEmpty().WithMessage($"{nameof(Vehicle.Year)} cannot be empty.")
			.GreaterThanOrEqualTo(1886u).WithMessage($"{nameof(Vehicle.Year)} must be greater than or equal to 1886.");
	}

	private static bool ValidateVinLength(uint year, string vin)
	{
		return year switch
		{
			>= 1981 => vin.Length >= 17,
			>= 1954 => vin.Length is >= 5 and <= 17,
			< 1954 => string.IsNullOrEmpty(vin),
		};
	}
}