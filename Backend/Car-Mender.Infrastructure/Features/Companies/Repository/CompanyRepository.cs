using Car_Mender.Domain.Common;
using Car_Mender.Domain.Entities;
using Car_Mender.Domain.Features.Companies.DTOs;
using Car_Mender.Domain.Features.Companies.Entities;
using Car_Mender.Domain.Features.Companies.Errors;
using Car_Mender.Domain.Features.Companies.Repository;
using Car_Mender.Infrastructure.Persistence.DatabaseContext;

namespace Car_Mender.Infrastructure.Features.Companies.Repository;

public class CompanyRepository(AppDbContext context) : ICompanyRepository
{
	public Task<Result<Company>> GetCompanyByIdAsync(Guid id)
	{
		throw new NotImplementedException();
	}

	public Task<Result<IEnumerable<Company>>> GetAllCompaniesAsync()
	{
		throw new NotImplementedException();
	}

	public async Task<Result> CreateCompanyAsync(CreateCompanyDto dto, CancellationToken cancellationToken)
	{
		var company = new Company
		{
			Email = dto.Email,
			Name = dto.Name,
			Address = dto.Address,
			Phone = dto.Phone,
			Nip = dto.Nip
		};
		
		try
		{
			await context.Companies.AddAsync(company, cancellationToken);
			await context.SaveChangesAsync(cancellationToken);
		}
		catch (Exception)
		{
			return Error.Unexpected;
		}

		return Result.Success();
	}

	public Task<Result> UpdateCompanyAsync(Company company)
	{
		throw new NotImplementedException();
	}

	public Task<Result> DeleteCompanyAsync(Guid id)
	{
		throw new NotImplementedException();
	}
}