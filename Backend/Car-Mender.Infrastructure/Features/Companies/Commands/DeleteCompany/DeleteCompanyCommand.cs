using Car_Mender.Domain.Common;
using MediatR;

namespace Car_Mender.Infrastructure.Features.Companies.Commands.DeleteCompany;

public record DeleteCompanyCommand(Guid Id) : IRequest<Result>;