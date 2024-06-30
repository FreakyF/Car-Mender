using Car_Mender.Domain.Features.Appointments.Entities;
using FluentValidation;

namespace Car_Mender.Infrastructure.Features.Appointments.Commands.CreateAppointment;

public class CreateAppointmentCommandValidator : AbstractValidator<CreateAppointmentCommand>
{
	public CreateAppointmentCommandValidator()
	{
		RuleFor(a => a.Date)
			.NotEmpty().WithMessage($"{nameof(Appointment.Date)} cannot be empty.")
			.Must(date => date.Date >= DateTime.Today)
			.WithMessage($"{nameof(Appointment.Date)} must be today or in the future.");

		RuleFor(a => a.Description)
			.NotEmpty().WithMessage($"{nameof(Appointment.Description)} cannot be empty.")
			.MaximumLength(1000)
			.WithMessage($"{nameof(Appointment.Description)} cannot be longer than 1000 characters.");

		RuleFor(a => a.AppointmentStatus)
			.IsInEnum().WithMessage(
				$"{nameof(Appointment.AppointmentStatus)}  must be a valid appointment status:  {{string.Join(\", \", Enum.GetNames(typeof(AppointmentStatus)))}}.");
	}
}