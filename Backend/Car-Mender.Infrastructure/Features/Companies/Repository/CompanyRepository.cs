using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Companies.Entities;
using Car_Mender.Domain.Features.Companies.Errors;
using Car_Mender.Domain.Features.Companies.Repository;
using Car_Mender.Infrastructure.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace Car_Mender.Infrastructure.Features.Companies.Repository;

public class CompanyRepository(AppDbContext context) : ICompanyRepository
{
	public async Task<Result<Company>> GetCompanyByIdAsync(Guid id, CancellationToken cancellationToken)
	{
		var company = await context.Companies.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

		return company is null
			? CompanyErrors.CouldNotBeFound
			: Result<Company>.Success(company);
	}

	public async Task<Result<Company>> GetCompanyByIdAsNoTrackingAsync(Guid id, CancellationToken cancellationToken)
	{
		var company = await context.Companies
			.AsNoTracking()
			.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

		return company is null
			? CompanyErrors.CouldNotBeFound
			: Result<Company>.Success(company);
	}

	public Task<Result<IEnumerable<Company>>> GetAllCompaniesAsync()
	{
		throw new NotImplementedException();
	}

	public async Task<Result<Guid>> CreateCompanyAsync(Company company, CancellationToken cancellationToken)
	{
		try
		{
			await context.Companies.AddAsync(company, cancellationToken);
			await context.SaveChangesAsync(cancellationToken);
		}
		catch (Exception)
		{
			return Error.Unexpected;
		}

		return Result<Guid>.Success(company.Id);
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