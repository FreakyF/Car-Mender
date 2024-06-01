using AutoMapper;
using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Companies.DTOs;
using Car_Mender.Domain.Features.Companies.Repository;
using MediatR;

namespace Car_Mender.Infrastructure.Features.Companies.Queries.GetCompanyQuery;

// ReSharper disable once UnusedType.Global
public class GetCompanyQueryHandler(
	ICompanyRepository repository,
	IMapper mapper
) : IRequestHandler<GetCompanyQuery, Result<GetCompanyDto>>
{
	public async Task<Result<GetCompanyDto>> Handle(GetCompanyQuery request, CancellationToken cancellationToken)
	{
		if (request.Id.Equals(Guid.Empty))
		{
			return Error.InvalidId;
		}

		var getCompanyResult = await repository.GetCompanyByIdAsNoTrackingAsync(request.Id);
		if (getCompanyResult.IsFailure)
		{
			return Result<GetCompanyDto>.Failure(getCompanyResult.Error);
		}

		var companyDto = mapper.Map<GetCompanyDto>(getCompanyResult.Value);
		return Result<GetCompanyDto>.Success(companyDto);
	}
}