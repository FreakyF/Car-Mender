using AutoMapper;
using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Branches.DTOs;
using Car_Mender.Domain.Features.Branches.Repository;
using MediatR;

namespace Car_Mender.Infrastructure.Features.Branches.Queries;

public class GetAllBranchesQueryHandler(
	IBranchRepository repository,
	IMapper mapper
) : IRequestHandler<GetAllBranchesQuery, Result<List<GetBranchDto>>>
{
	public async Task<Result<List<GetBranchDto>>> Handle(GetAllBranchesQuery request, CancellationToken cancellationToken)
	{
		var getBranchesResult = await repository.GetAllBranchesAsync(cancellationToken);
		if (getBranchesResult.IsFailure) return Result<List<GetBranchDto>>.Failure(getBranchesResult.Error);

		var branchesDtos = mapper.Map<List<GetBranchDto>>(getBranchesResult.Value);
		return Result<List<GetBranchDto>>.Success(branchesDtos);
	}
}