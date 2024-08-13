using AutoMapper;
using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Engines.DTOs;
using Car_Mender.Domain.Features.Issues.DTOs;
using Car_Mender.Domain.Features.Issues.Repository;
using Car_Mender.Infrastructure.Features.Engines.Queries.GetAllEnginesQuery;
using MediatR;

namespace Car_Mender.Infrastructure.Features.Issues.Queries;

public class GetAllIssuesQueryHandler(
	IIssueRepository repository,
	IMapper mapper
) : IRequestHandler<GetAllIssuesQuery, Result<List<GetIssueDto>>>
{
	public async Task<Result<List<GetIssueDto>>> Handle(GetAllIssuesQuery request,
		CancellationToken cancellationToken)
	{
		var getIssuesResult = await repository.GetAllIssuesAsync(request.VehicleId, cancellationToken);
		if (getIssuesResult.IsFailure) return Result<List<GetIssueDto>>.Failure(getIssuesResult.Error);

		var issuesDtos = mapper.Map<List<GetIssueDto>>(getIssuesResult.Value);
		return Result<List<GetIssueDto>>.Success(issuesDtos);
	}
}