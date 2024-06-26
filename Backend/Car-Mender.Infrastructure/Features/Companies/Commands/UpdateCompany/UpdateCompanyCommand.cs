using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Companies.DTOs;
using Car_Mender.Domain.Features.Companies.DTOs.Company;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace Car_Mender.Infrastructure.Features.Companies.Commands.UpdateCompany;

// ReSharper disable once ClassNeverInstantiated.Global
public record UpdateCompanyCommand(Guid Id, JsonPatchDocument<UpdateCompanyDto> PatchDocument) : IRequest<Result>;