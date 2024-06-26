using AutoMapper;
using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Branches.DTOs;
using Car_Mender.Domain.Features.Branches.Repository;
using Car_Mender.Domain.Features.Companies.DTOs;
using Car_Mender.Infrastructure.Features.Companies.Queries.GetCompanyQuery;
using MediatR;

namespace Car_Mender.Infrastructure.Features.Branches.Queries;

public class GetBranchQueryHandler(
	IBranchRepository repository,
	IMapper mapper
) : IRequestHandler<GetBranchQuery, Result<GetBranchDto>>
{
	public async Task<Result<GetBranchDto>> Handle(GetBranchQuery request, CancellationToken cancellationToken)
	{
		if (request.Id.Equals(Guid.Empty))
		{
			return Error.InvalidId;
		}

		var getBranchResult = await repository.GetBranchByIdNoTrackingAsync(request.Id, cancellationToken);
		if (getBranchResult.IsFailure)
		{
			return Result<GetBranchDto>.Failure(getBranchResult.Error);
		}

		var branchDto = mapper.Map<GetBranchDto>(getBranchResult.Value);
		return Result<GetBranchDto>.Success(branchDto);
	}
}