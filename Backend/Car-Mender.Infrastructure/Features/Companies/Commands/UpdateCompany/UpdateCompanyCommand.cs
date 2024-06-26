using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Companies.DTOs;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace Car_Mender.Infrastructure.Features.Companies.Commands.UpdateCompany;

public record UpdateCompanyCommand(Guid Id, JsonPatchDocument<UpdateCompanyDto> PatchDocument) : IRequest<Result>;