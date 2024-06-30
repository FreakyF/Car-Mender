using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Vehicles.Entities;
using Car_Mender.Domain.Features.Vehicles.Errors;
using Car_Mender.Domain.Features.Vehicles.Repository;
using Car_Mender.Infrastructure.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace Car_Mender.Infrastructure.Features.Vehicles.Repository;

public class VehicleRepository(AppDbContext context) : IVehicleRepository
{
	public async Task<Result<Vehicle>> GetVehicleByIdAsync(Guid id, CancellationToken cancellationToken)
	{
		var vehicle = await context.Vehicles.FirstOrDefaultAsync(v => v.Id == id, cancellationToken);

		return vehicle is null
			? VehicleErrors.CouldNotBeFound
			: Result<Vehicle>.Success(vehicle);
	}

	public async Task<Result<Vehicle>> GetVehicleByIdNoTrackingAsync(Guid id, CancellationToken cancellationToken)
	{
		var vehicle = await context.Vehicles
			.AsNoTracking()
			.FirstOrDefaultAsync(v => v.Id == id, cancellationToken);

		return vehicle is null
			? VehicleErrors.CouldNotBeFound
			: Result<Vehicle>.Success(vehicle);
	}

	public Task<IEnumerable<Vehicle>> GetAllVehiclesAsync()
	{
		throw new NotImplementedException();
	}

	public async Task<Result<Guid>> CreateVehicleAsync(Vehicle vehicle, CancellationToken cancellationToken)
	{
		try
		{
			await context.Vehicles.AddAsync(vehicle, cancellationToken);
			await context.SaveChangesAsync(cancellationToken);
		}
		catch (Exception)
		{
			return Error.Unexpected;
		}

		return Result<Guid>.Success(vehicle.Id);
	}

	public Task<Result> UpdateVehicleAsync(Vehicle vehicle, CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}

	public async Task<Result> DeleteVehicleAsync(Guid id, CancellationToken cancellationToken)
	{
		var getVehicleResult = await GetVehicleByIdAsync(id, cancellationToken);
		if (getVehicleResult.IsFailure) return Result.Failure(getVehicleResult.Error);

		var vehicle = getVehicleResult.Value;
		context.Vehicles.Remove(vehicle!);
		await context.SaveChangesAsync(cancellationToken);
		return Result.Success();
	}

	public async Task<Result<bool>> ExistsAsync(Guid id)
	{
		var exists = await context.Vehicles.AnyAsync(e => e.Id == id);
		return Result<bool>.Success(exists);
	}

	public async Task<Result<int>> SaveChangesAsync(CancellationToken cancellationToken)
	{
		var writtenEntities = await context.SaveChangesAsync(cancellationToken);
		return Result<int>.Success(writtenEntities);
	}
}