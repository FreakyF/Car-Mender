using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Appointments.DTOs;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace Car_Mender.Infrastructure.Features.Appointments.Commands.UpdateAppointment;

public record UpdateAppointmentCommand(Guid Id, JsonPatchDocument<UpdateAppointmentDto> PatchDocument)
	: IRequest<Result>;