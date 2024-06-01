using AutoMapper;
using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Companies.DTOs;
using Car_Mender.Domain.Features.Companies.Repository;
using FluentValidation;
using MediatR;

namespace Car_Mender.Infrastructure.Features.Companies.Commands.CreateCompany;

// ReSharper disable once UnusedType.Global
public class CreateCompanyCommandHandler(
	ICompanyRepository companyRepository,
	IValidator<CreateCompanyCommand> validator,
	IMapper mapper
) : IRequestHandler<CreateCompanyCommand, Result>
{
	public async Task<Result> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
	{
		var validationResult = await validator.ValidateAsync(request, cancellationToken);
		if (!validationResult.IsValid)
		{
			var errors = validationResult.Errors.Select(e => e.ErrorMessage);
			return Error.ValidationError(errors);
		}

		var dto = mapper.Map<CreateCompanyDto>(request);
		return await companyRepository.CreateCompanyAsync(dto, cancellationToken);
	}
}