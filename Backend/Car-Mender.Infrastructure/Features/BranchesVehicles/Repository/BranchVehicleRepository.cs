using Car_Mender.Domain.Common;
using Car_Mender.Domain.Entities;
using Car_Mender.Domain.Features.BranchesVehicles.Errors;
using Car_Mender.Domain.Repositories;
using Car_Mender.Infrastructure.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace Car_Mender.Infrastructure.Repositories;

public class BranchVehicleRepository(AppDbContext context) : IBranchVehicleRepository
{
	public async Task<Result<BranchVehicle>> GetBranchVehicleByIdAsync(Guid id, CancellationToken cancellationToken)
	{
		var branchVehicle = await context.BranchesVehicles.FirstOrDefaultAsync(b => b.Id == id, cancellationToken);

		return branchVehicle is null
			? BranchVehiclesErrors.CouldNotBeFound
			: Result<BranchVehicle>.Success(branchVehicle);
	}

	public async Task<Result<BranchVehicle>> GetBranchVehicleByNoTrackingIdAsync(Guid id,
		CancellationToken cancellationToken)
	{
		var branchVehicle = await context.BranchesVehicles
			.AsNoTracking()
			.FirstOrDefaultAsync(b => b.Id == id, cancellationToken);

		return branchVehicle is null
			? BranchVehiclesErrors.CouldNotBeFound
			: Result<BranchVehicle>.Success(branchVehicle);
	}

	public async Task<Result<IEnumerable<BranchVehicle>>> GetAllBranchVehiclesAsync(CancellationToken cancellationToken)
	{
		var branchesVehicles = await context.BranchesVehicles.ToListAsync(cancellationToken);

		return branchesVehicles.Count == 0
			? BranchVehiclesErrors.CouldNotBeFound
			: Result<IEnumerable<BranchVehicle>>.Success(branchesVehicles);
	}

	public async Task<Result<Guid>> CreateBranchVehicleAsync(BranchVehicle branchVehicle,
		CancellationToken cancellationToken)
	{
		try
		{
			await context.BranchesVehicles.AddAsync(branchVehicle, cancellationToken);
			await context.SaveChangesAsync(cancellationToken);
		}
		catch (Exception)
		{
			return Error.Unexpected;
		}

		return Result<Guid>.Success(branchVehicle.Id);
	}

	public async Task<Result> DeleteBranchVehicleAsync(Guid id, CancellationToken cancellationToken)
	{
		var getBranchVehicleResult = await GetBranchVehicleByIdAsync(id, cancellationToken);
		if (getBranchVehicleResult.IsFailure) return Result.Failure(getBranchVehicleResult.Error);

		var branchVehicle = getBranchVehicleResult.Value;
		context.BranchesVehicles.Remove(branchVehicle!);
		await context.SaveChangesAsync(cancellationToken);
		return Result.Success();
	}

	public async Task<Result<bool>> ExistsAsync(Guid id)
	{
		var exists = await context.BranchesVehicles.AnyAsync(b => b.Id == id);
		return Result<bool>.Success(exists);
	}

	public async Task<Result<int>> SaveChangesAsync(CancellationToken cancellationToken)
	{
		var writtenEntities = await context.SaveChangesAsync(cancellationToken);
		return Result<int>.Success(writtenEntities);
	}
}