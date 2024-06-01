using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Companies.DTOs;
using MediatR;

namespace Car_Mender.Infrastructure.Features.Companies.Queries.GetCompanyQuery;

public record GetCompanyQuery(Guid Id) : IRequest<Result<GetCompanyDto>>;