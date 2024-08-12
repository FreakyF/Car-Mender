using Car_Mender.Domain.Common;
using Car_Mender.Domain.Entities;

namespace Car_Mender.Domain.Repositories;

public interface IBranchVehicleRepository
{
	Task<Result<BranchVehicle>> GetBranchVehicleByIdAsync(Guid id, CancellationToken cancellationToken);
	Task<Result<BranchVehicle>> GetBranchVehicleByNoTrackingIdAsync(Guid id, CancellationToken cancellationToken);
	Task<Result<IEnumerable<BranchVehicle>>> GetAllBranchVehiclesAsync(CancellationToken cancellationToken);
	Task<Result<Guid>> CreateBranchVehicleAsync(BranchVehicle branchVehicle, CancellationToken cancellationToken);
	Task<Result> DeleteBranchVehicleAsync(Guid id, CancellationToken cancellationToken);
	Task<Result<bool>> ExistsAsync(Guid id);
	Task<Result<int>> SaveChangesAsync(CancellationToken cancellationToken);
}