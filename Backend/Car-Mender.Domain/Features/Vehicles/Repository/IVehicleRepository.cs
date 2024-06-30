using Car_Mender.Domain.Features.Vehicles.Entities;

namespace Car_Mender.Domain.Features.Vehicles.Repository;

public interface IVehicleRepository
{
	Task<Vehicle> GetVehicleByIdAsync(Guid id);
	Task<IEnumerable<Vehicle>> GetAllVehiclesAsync();
	Task AddVehicleAsync(Vehicle vehicle);
	Task UpdateVehicleAsync(Vehicle vehicle);
	Task DeleteVehicleAsync(Guid id);
}