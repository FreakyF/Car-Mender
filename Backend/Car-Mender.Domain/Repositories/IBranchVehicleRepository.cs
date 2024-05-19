using Car_Mender.Domain.Entities;

namespace Car_Mender.Domain.Repositories;

public interface IBranchVehicleRepository
{
    Task<BranchVehicle> GetBranchVehicleByIdAsync(Guid id);
}