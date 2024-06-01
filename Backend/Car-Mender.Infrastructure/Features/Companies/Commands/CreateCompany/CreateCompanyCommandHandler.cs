using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Companies.DTOs;
using Car_Mender.Domain.Features.Companies.Repository;
using FluentValidation;
using MediatR;

namespace Car_Mender.Infrastructure.Features.Companies.Commands.CreateCompany;

public class CreateCompanyCommandHandler(
	ICompanyRepository companyRepository,
	IValidator<CreateCompanyCommand> validator) : IRequestHandler<CreateCompanyCommand, Result>
{
	public async Task<Result> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
	{
		var validationResult = await validator.ValidateAsync(request, cancellationToken);
		if (!validationResult.IsValid)
		{
			var errors = validationResult.Errors.Select(e => e.ErrorMessage);
			return Error.ValidationError(errors);
		}
		
		var dto = new CreateCompanyDto
		{
			Email = request.Email,
			Name = request.Name,
			Address = request.Address,
			Phone = request.Phone,
			Nip = request.Nip
		};

		return await companyRepository.CreateCompanyAsync(dto, cancellationToken);
	}
}