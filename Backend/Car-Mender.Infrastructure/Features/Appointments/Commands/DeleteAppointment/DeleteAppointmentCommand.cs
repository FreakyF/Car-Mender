using Car_Mender.Domain.Common;
using MediatR;

namespace Car_Mender.Infrastructure.Features.Appointments.Commands.DeleteAppointment;

public record DeleteAppointmentCommand(Guid Id) : IRequest<Result>;