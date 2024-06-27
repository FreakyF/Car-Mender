using AutoMapper;
using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Companies.Entities;
using Car_Mender.Domain.Features.Companies.Errors;
using Car_Mender.Domain.Features.Companies.Repository;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace Car_Mender.Infrastructure.Features.Companies.Commands.UpdateCompany;

public class UpdateCompanyCommandHandler(
	ICompanyRepository companyRepository,
	IValidator<UpdateCompanyCommand> validator,
	IMapper mapper
) : IRequestHandler<UpdateCompanyCommand, Result>
{
	public async Task<Result> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
	{
		var companyExistsResult = await companyRepository.ExistsAsync(request.Id);
		if (!companyExistsResult.Value) return CompanyErrors.CouldNotBeFound;

		var validationResult = await validator.ValidateAsync(request, cancellationToken);
		if (!validationResult.IsValid)
		{
			var errors = validationResult.Errors.Select(e => e.ErrorMessage);
			return Error.ValidationError(errors);
		}

		var getCompanyResult = await companyRepository.GetCompanyByIdAsync(request.Id, cancellationToken);
		var patchDoc = mapper.Map<JsonPatchDocument<Company>>(request.PatchDocument);
		patchDoc.ApplyTo(getCompanyResult.Value!);
		await companyRepository.SaveChangesAsync(cancellationToken);

		return Result.Success();
	}
}