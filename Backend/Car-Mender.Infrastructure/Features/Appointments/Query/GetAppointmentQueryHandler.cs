using AutoMapper;
using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Appointments.DTOs;
using Car_Mender.Domain.Features.Appointments.Repository;
using MediatR;

namespace Car_Mender.Infrastructure.Features.Appointments.Query;

public class GetAppointmentQueryHandler(
	IAppointmentRepository repository,
	IMapper mapper
) : IRequestHandler<GetAppointmentQuery, Result<GetAppointmentDto>>
{
	public async Task<Result<GetAppointmentDto>> Handle(GetAppointmentQuery request,
		CancellationToken cancellationToken)
	{
		if (request.Id.Equals(Guid.Empty)) return Error.InvalidId;

		var getAppointmentResult = await repository.GetAppointmentByIdNoTrackingAsync(request.Id, cancellationToken);
		if (getAppointmentResult.IsFailure) return Result<GetAppointmentDto>.Failure(getAppointmentResult.Error);

		var branchDto = mapper.Map<GetAppointmentDto>(getAppointmentResult.Value);
		return Result<GetAppointmentDto>.Success(branchDto);
	}
}