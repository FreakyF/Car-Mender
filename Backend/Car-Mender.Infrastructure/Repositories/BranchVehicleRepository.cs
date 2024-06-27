using Car_Mender.Domain.Entities;
using Car_Mender.Domain.Repositories;
using Car_Mender.Infrastructure.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace Car_Mender.Infrastructure.Repositories;

public class BranchVehicleRepository(AppDbContext context) : IBranchVehicleRepository
{
	public async Task<BranchVehicle> GetBranchVehicleByIdAsync(Guid id)
	{
		return await context.Set<BranchVehicle>().FirstOrDefaultAsync(a => a.Id == id);
	}

	public async Task<IEnumerable<BranchVehicle>> GetAllBranchVehiclesAsync()
	{
		return await context.Set<BranchVehicle>().ToListAsync();
	}

	public async Task AddBranchVehicleAsync(BranchVehicle branchVehicle)
	{
		await context.Set<BranchVehicle>().AddAsync(branchVehicle);
		await context.SaveChangesAsync();
	}

	public async Task UpdateBranchVehicleAsync(BranchVehicle branchVehicle)
	{
		context.Set<BranchVehicle>().Update(branchVehicle);
		await context.SaveChangesAsync();
	}

	public async Task DeleteBranchVehicleAsync(Guid id)
	{
		var branchVehicle = await GetBranchVehicleByIdAsync(id);
		context.Set<BranchVehicle>().Remove(branchVehicle);
		await context.SaveChangesAsync();
	}
}