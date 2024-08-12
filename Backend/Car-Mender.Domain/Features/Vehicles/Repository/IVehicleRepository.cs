using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Vehicles.Entities;

namespace Car_Mender.Domain.Features.Vehicles.Repository;

public interface IVehicleRepository
{
	Task<Result<Vehicle>> GetVehicleByIdAsync(Guid id, CancellationToken cancellationToken);
	Task<Result<Vehicle>> GetVehicleByIdNoTrackingAsync(Guid id, CancellationToken cancellationToken);
	Task<Result<IEnumerable<Vehicle>>> GetAllVehiclesAsync(CancellationToken cancellationToken);
	Task<Result<Guid>> CreateVehicleAsync(Vehicle vehicle, CancellationToken cancellationToken);
	Task<Result> DeleteVehicleAsync(Guid id, CancellationToken cancellationToken);
	Task<Result<bool>> ExistsAsync(Guid id);
	Task<Result<int>> SaveChangesAsync(CancellationToken cancellationToken);
}