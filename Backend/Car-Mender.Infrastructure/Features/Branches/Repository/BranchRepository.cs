using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Branches.Entities;
using Car_Mender.Domain.Features.Branches.Errors;
using Car_Mender.Domain.Features.Branches.Repository;
using Car_Mender.Infrastructure.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace Car_Mender.Infrastructure.Features.Branches.Repository;

public class BranchRepository(AppDbContext context) : IBranchRepository
{
	public async Task<Result<Branch>> GetBranchByIdAsync(Guid id, CancellationToken cancellationToken)
	{
		var branch = await context.Branches.FirstOrDefaultAsync(b => b.Id == id, cancellationToken);

		return branch is null
			? BranchErrors.CouldNotBeFound
			: Result<Branch>.Success(branch);
	}

	public async Task<Result<Branch>> GetBranchByIdNoTrackingAsync(Guid id, CancellationToken cancellationToken)
	{
		var branch = await context.Branches
			.AsNoTracking()
			.FirstOrDefaultAsync(b => b.Id == id, cancellationToken);

		return branch is null
			? BranchErrors.CouldNotBeFound
			: Result<Branch>.Success(branch);
	}

	public async Task<Result<IEnumerable<Branch>>> GetAllBranchesAsync(Guid companyId,
		CancellationToken cancellationToken)
	{
		var branches = await context.Branches
			.Where(w => w.CompanyId == companyId)
			.ToListAsync(cancellationToken);

		return branches.Count == 0
			? BranchErrors.CouldNotBeFound
			: Result<IEnumerable<Branch>>.Success(branches);
	}

	public async Task<Result<Guid>> CreateBranchAsync(Branch branch, CancellationToken cancellationToken)
	{
		try
		{
			await context.Branches.AddAsync(branch, cancellationToken);
			await context.SaveChangesAsync(cancellationToken);
		}
		catch (Exception)
		{
			return Error.Unexpected;
		}

		return Result<Guid>.Success(branch.Id);
	}

	public async Task<Result> DeleteBranchAsync(Guid id, CancellationToken cancellationToken)
	{
		var getBranchResult = await GetBranchByIdAsync(id, cancellationToken);
		if (getBranchResult.IsFailure) return Result.Failure(getBranchResult.Error);

		var branch = getBranchResult.Value;
		context.Branches.Remove(branch!);
		await context.SaveChangesAsync(cancellationToken);
		return Result.Success();
	}

	public async Task<Result<bool>> ExistsAsync(Guid id)
	{
		var exists = await context.Branches.AnyAsync(b => b.Id == id);
		return Result<bool>.Success(exists);
	}

	public async Task<Result<int>> SaveChangesAsync(CancellationToken cancellationToken)
	{
		var writtenEntities = await context.SaveChangesAsync(cancellationToken);
		return Result<int>.Success(writtenEntities);
	}
}