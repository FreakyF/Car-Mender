using AutoMapper;
using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Vehicles.DTOs;
using Car_Mender.Domain.Features.Vehicles.Repository;
using MediatR;

namespace Car_Mender.Infrastructure.Features.Vehicles.Queries;

public class GetVehicleQueryHandler(
	IVehicleRepository repository,
	IMapper mapper
) : IRequestHandler<GetVehicleQuery, Result<GetVehicleDto>>
{
	public async Task<Result<GetVehicleDto>> Handle(GetVehicleQuery request, CancellationToken cancellationToken)
	{
		if (request.Id.Equals(Guid.Empty)) return Error.InvalidId;

		var getWorkerResult = await repository.GetVehicleByIdNoTrackingAsync(request.Id, cancellationToken);
		if (getWorkerResult.IsFailure) return Result<GetVehicleDto>.Failure(getWorkerResult.Error);

		var workerDto = mapper.Map<GetVehicleDto>(getWorkerResult.Value);
		return Result<GetVehicleDto>.Success(workerDto);
	}
}