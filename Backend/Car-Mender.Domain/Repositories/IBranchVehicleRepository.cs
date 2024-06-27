using Car_Mender.Domain.Entities;

namespace Car_Mender.Domain.Repositories;

public interface IBranchVehicleRepository
{
	Task<BranchVehicle> GetBranchVehicleByIdAsync(Guid id);
	Task<IEnumerable<BranchVehicle>> GetAllBranchVehiclesAsync();
	Task AddBranchVehicleAsync(BranchVehicle branchVehicle);
	Task UpdateBranchVehicleAsync(BranchVehicle branchVehicle);
	Task DeleteBranchVehicleAsync(Guid id);
}