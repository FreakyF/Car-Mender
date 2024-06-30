using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Appointments.DTOs;
using MediatR;

namespace Car_Mender.Infrastructure.Features.Appointments.Query;

public record GetAppointmentQuery(Guid Id) : IRequest<Result<GetAppointmentDto>>;