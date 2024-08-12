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

	public async Task<Result> DeleteCompanyAsync(Guid id, CancellationToken cancellationToken)
	{
		var getCompanyResult = await GetCompanyByIdAsync(id, cancellationToken);
		if (getCompanyResult.IsFailure) return Result.Failure(getCompanyResult.Error);

		var company = getCompanyResult.Value;
		context.Companies.Remove(company!);
		await context.SaveChangesAsync(cancellationToken);
		return Result.Success();
	}

	public async Task<Result<bool>> ExistsAsync(Guid id)
	{
		var exists = await context.Companies.AnyAsync(c => c.Id == id);
		return Result<bool>.Success(exists);
	}

	public async Task<Result<int>> SaveChangesAsync(CancellationToken cancellationToken)
	{
		var writtenEntities = await context.SaveChangesAsync(cancellationToken);
		return Result<int>.Success(writtenEntities);
	}
}