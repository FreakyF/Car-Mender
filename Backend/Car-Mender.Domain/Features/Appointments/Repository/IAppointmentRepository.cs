using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Appointments.Entities;

namespace Car_Mender.Domain.Features.Appointments.Repository;

public interface IAppointmentRepository
{
	Task<Result<Appointment>> GetAppointmentByIdAsync(Guid id, CancellationToken cancellationToken);
	Task<Result<Appointment>> GetAppointmentByIdNoTrackingAsync(Guid id, CancellationToken cancellationToken);
	Task<Result<IEnumerable<Appointment>>> GetAllAppointmentsAsync(CancellationToken cancellationToken);
	Task<Result<Guid>> CreateAppointmentAsync(Appointment appointment, CancellationToken cancellationToken);
	Task<Result> UpdateAppointmentAsync(Appointment appointment, CancellationToken cancellationToken);
	Task<Result> DeleteAppointmentAsync(Guid id, CancellationToken cancellationToken);
	Task<Result<bool>> ExistsAsync(Guid id);
	Task<Result<int>> SaveChangesAsync(CancellationToken cancellationToken);
}