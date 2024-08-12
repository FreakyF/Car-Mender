using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Branches.DTOs;
using MediatR;

namespace Car_Mender.Infrastructure.Features.Branches.Queries;

public record GetAllBranchesQuery(Guid CompanyId) : IRequest<Result<List<GetBranchDto>>>;