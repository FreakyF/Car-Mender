using Car_Mender.Domain.Features.Vehicles.Entities;
using Car_Mender.Domain.Features.Vehicles.Repository;
using Car_Mender.Infrastructure.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace Car_Mender.Infrastructure.Features.Vehicles.Repository;

public class VehicleRepository(AppDbContext context) : IVehicleRepository
{
	public async Task<Vehicle> GetVehicleByIdAsync(Guid id)
	{
		return await context.Set<Vehicle>().FirstOrDefaultAsync(a => a.Id == id);
	}

	public async Task<IEnumerable<Vehicle>> GetAllVehiclesAsync()
	{
		return await context.Set<Vehicle>().ToListAsync();
	}

	public async Task AddVehicleAsync(Vehicle vehicle)
	{
		await context.Set<Vehicle>().AddAsync(vehicle);
		await context.SaveChangesAsync();
	}

	public async Task UpdateVehicleAsync(Vehicle vehicle)
	{
		context.Set<Vehicle>().Update(vehicle);
		await context.SaveChangesAsync();
	}

	public async Task DeleteVehicleAsync(Guid id)
	{
		var vehicle = await GetVehicleByIdAsync(id);
		context.Set<Vehicle>().Remove(vehicle);
		await context.SaveChangesAsync();
	}
}