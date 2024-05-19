using Car_Mender.Domain.Entities;

namespace Car_Mender.Domain.Repositories;

public interface IVehicleRepository
{
    Task<Vehicle> GetVehicleByIdAsync(Guid id);
}