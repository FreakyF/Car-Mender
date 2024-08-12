using AutoMapper;
using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Branches.DTOs;
using Car_Mender.Domain.Features.Workers.DTOs;
using Car_Mender.Domain.Features.Workers.Repository;
using MediatR;

namespace Car_Mender.Infrastructure.Features.Workers.Queries;

public class GetAllWorkersQueryHandler(
	IWorkerRepository repository,
	IMapper mapper
) : IRequestHandler<GetAllWorkersQuery, Result<List<GetWorkerDto>>>
{
	public async Task<Result<List<GetWorkerDto>>> Handle(GetAllWorkersQuery request, CancellationToken cancellationToken)
	{
		var getWorkersResult = await repository.GetAllWorkersAsync(request.BranchId, cancellationToken);
		if (getWorkersResult.IsFailure) return Result<List<GetWorkerDto>>.Failure(getWorkersResult.Error);

		var workersDto = mapper.Map<List<GetWorkerDto>>(getWorkersResult.Value);
		return Result<List<GetWorkerDto>>.Success(workersDto);
	}
}