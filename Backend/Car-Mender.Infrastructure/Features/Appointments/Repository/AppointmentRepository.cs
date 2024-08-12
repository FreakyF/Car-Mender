using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Appointments.Entities;
using Car_Mender.Domain.Features.Appointments.Errors;
using Car_Mender.Domain.Features.Appointments.Repository;
using Car_Mender.Infrastructure.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace Car_Mender.Infrastructure.Features.Appointments.Repository;

public class AppointmentRepository(AppDbContext context) : IAppointmentRepository
{
	public async Task<Result<Appointment>> GetAppointmentByIdAsync(Guid id, CancellationToken cancellationToken)
	{
		var appointment = await context.Appointments
			.FirstOrDefaultAsync(v => v.Id == id, cancellationToken);

		return appointment is null
			? AppointmentErrors.CouldNotBeFound
			: Result<Appointment>.Success(appointment);
	}

	public async Task<Result<Appointment>> GetAppointmentByIdNoTrackingAsync(Guid id,
		CancellationToken cancellationToken)
	{
		var appointment = await context.Appointments
			.AsNoTracking()
			.FirstOrDefaultAsync(v => v.Id == id, cancellationToken);

		return appointment is null
			? AppointmentErrors.CouldNotBeFound
			: Result<Appointment>.Success(appointment);
	}

	public async Task<Result<IEnumerable<Appointment>>> GetAllAppointmentsAsync(CancellationToken cancellationToken)
	{
		var appointments = await context.Appointments.ToListAsync(cancellationToken);

		return appointments.Count == 0
			? AppointmentErrors.CouldNotBeFound
			: Result<IEnumerable<Appointment>>.Success(appointments);
	}

	public async Task<Result<Guid>> CreateAppointmentAsync(Appointment appointment, CancellationToken cancellationToken)
	{
		try
		{
			await context.Appointments.AddAsync(appointment, cancellationToken);
			await context.SaveChangesAsync(cancellationToken);
		}
		catch (Exception)
		{
			return Error.Unexpected;
		}

		return Result<Guid>.Success(appointment.Id);
	}

	public Task<Result> UpdateAppointmentAsync(Appointment appointment, CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}

	public async Task<Result> DeleteAppointmentAsync(Guid id, CancellationToken cancellationToken)
	{
		var getAppointmentResult = await GetAppointmentByIdAsync(id, cancellationToken);
		if (getAppointmentResult.IsFailure) return Result.Failure(getAppointmentResult.Error);

		var appointment = getAppointmentResult.Value;
		context.Appointments.Remove(appointment!);
		await context.SaveChangesAsync(cancellationToken);
		return Result.Success();
	}

	public async Task<Result<bool>> ExistsAsync(Guid id)
	{
		var exists = await context.Appointments.AnyAsync(e => e.Id == id);
		return Result<bool>.Success(exists);
	}

	public async Task<Result<int>> SaveChangesAsync(CancellationToken cancellationToken)
	{
		var writtenEntities = await context.SaveChangesAsync(cancellationToken);
		return Result<int>.Success(writtenEntities);
	}
}