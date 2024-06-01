using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Companies.DTOs;
using Car_Mender.Domain.Features.Companies.Repository;
using MediatR;

namespace Car_Mender.Infrastructure.Features.Companies.Commands.CreateCompany;

public class CreateCompanyCommandHandler(ICompanyRepository companyRepository) : IRequestHandler<CreateCompanyCommand, Result>
{
    public async Task<Result> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        // TODO: Validate request
        
        // TODO: Setup AutoMapper
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