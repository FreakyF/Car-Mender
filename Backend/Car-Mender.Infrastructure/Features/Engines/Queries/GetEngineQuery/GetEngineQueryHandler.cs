using AutoMapper;
using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Engines.DTOs;
using Car_Mender.Domain.Features.Engines.Repository;
using MediatR;

namespace Car_Mender.Infrastructure.Features.Engines.Queries.GetEngineQuery;

public class GetEngineQueryHandler(
	IEngineRepository repository,
	IMapper mapper
) : IRequestHandler<GetEngineQuery, Result<GetEngineDto>>
{
	public async Task<Result<GetEngineDto>> Handle(GetEngineQuery request, CancellationToken cancellationToken)
	{
		if (request.Id.Equals(Guid.Empty))
		{
			return Error.InvalidId;
		}

		var getEngineResult = await repository.GetEngineByIdNoTrackingAsync(request.Id, cancellationToken);
		if (getEngineResult.IsFailure)
		{
			return Result<GetEngineDto>.Failure(getEngineResult.Error);
		}

		var workerDto = mapper.Map<GetEngineDto>(getEngineResult.Value);
		return Result<GetEngineDto>.Success(workerDto);
	}
}