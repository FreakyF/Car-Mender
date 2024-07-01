using AutoMapper;
using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Issues.DTOs;
using Car_Mender.Domain.Features.Issues.Repository;
using MediatR;

namespace Car_Mender.Infrastructure.Features.Issues.Queries;

public class GetIssueQueryHandler(
	IIssueRepository repository,
	IMapper mapper
) : IRequestHandler<GetIssueQuery, Result<GetIssueDto>>
{
	public async Task<Result<GetIssueDto>> Handle(GetIssueQuery request, CancellationToken cancellationToken)
	{
		if (request.Id.Equals(Guid.Empty)) return Error.InvalidId;

		var getIssueResult = await repository.GetIssueByIdNoTrackingAsync(request.Id, cancellationToken);
		if (getIssueResult.IsFailure) return Result<GetIssueDto>.Failure(getIssueResult.Error);

		var workerDto = mapper.Map<GetIssueDto>(getIssueResult.Value);
		return Result<GetIssueDto>.Success(workerDto);
	}
}