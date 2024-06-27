using AutoMapper;
using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Workers.DTOs;
using Car_Mender.Domain.Features.Workers.Repository;
using MediatR;

namespace Car_Mender.Infrastructure.Features.Workers.Queries;

public class GetWorkerQueryHandler(
	IWorkerRepository repository,
	IMapper mapper
) : IRequestHandler<GetWorkerQuery, Result<GetWorkerDto>>
{
	public async Task<Result<GetWorkerDto>> Handle(GetWorkerQuery request, CancellationToken cancellationToken)
	{
		if (request.Id.Equals(Guid.Empty))
		{
			return Error.InvalidId;
		}

		var getWorkerResult = await repository.GetWorkerByIdNoTrackingAsync(request.Id, cancellationToken);
		if (getWorkerResult.IsFailure)
		{
			return Result<GetWorkerDto>.Failure(getWorkerResult.Error);
		}

		var workerDto = mapper.Map<GetWorkerDto>(getWorkerResult.Value);
		return Result<GetWorkerDto>.Success(workerDto);
	}
}