using AutoMapper;
using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.BranchesVehicles.DTOs;
using Car_Mender.Domain.Repositories;
using MediatR;

namespace Car_Mender.Infrastructure.Features.BranchesVehicles.Queries;

public class GetBranchVehicleQueryHandler(
	IBranchVehicleRepository repository,
	IMapper mapper
) : IRequestHandler<GetBranchVehicleQuery, Result<GetBranchVehicleDto>>
{
	public async Task<Result<GetBranchVehicleDto>> Handle(GetBranchVehicleQuery request, CancellationToken cancellationToken)
	{
		if (request.Id.Equals(Guid.Empty)) return Error.InvalidId;

		var getBranchVehicleResult = await repository.GetBranchVehicleByNoTrackingIdAsync(request.Id, cancellationToken);
		if (getBranchVehicleResult.IsFailure) return Result<GetBranchVehicleDto>.Failure(getBranchVehicleResult.Error);

		var branchDto = mapper.Map<GetBranchVehicleDto>(getBranchVehicleResult.Value);
		return Result<GetBranchVehicleDto>.Success(branchDto);
	}
}