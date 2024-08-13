using AutoMapper;
using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Engines.DTOs;
using Car_Mender.Domain.Features.Engines.Repository;
using MediatR;

namespace Car_Mender.Infrastructure.Features.Engines.Queries.GetAllEnginesQuery;

public class GetAllEnginesQueryHandler(
	IEngineRepository repository,
	IMapper mapper
) : IRequestHandler<GetAllEnginesQuery, Result<List<GetEngineDto>>>
{
	public async Task<Result<List<GetEngineDto>>> Handle(GetAllEnginesQuery request,
		CancellationToken cancellationToken)
	{
		var getEnginesResult = await repository.GetAllEnginesAsync(request.VehicleId, cancellationToken);
		if (getEnginesResult.IsFailure) return Result<List<GetEngineDto>>.Failure(getEnginesResult.Error);

		var enginesDtos = mapper.Map<List<GetEngineDto>>(getEnginesResult.Value);
		return Result<List<GetEngineDto>>.Success(enginesDtos);
	}
}