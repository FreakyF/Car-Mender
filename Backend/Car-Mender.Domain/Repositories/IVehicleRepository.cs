using Car_Mender.Domain.Entities;

namespace Car_Mender.Domain.Repositories;

public interface IVehicleRepository
{
    Task<Vehicle> GetVehicleByIdAsync(Guid id);
    Task<IEnumerable<Vehicle>> GetAllVehiclesAsync();
    Task AddVehicleAsync(Vehicle vehicle);
    Task UpdateVehicleAsync(Vehicle vehicle);
    Task DeleteVehicleAsync(Guid id);
}