using Car_Mender.Domain.Common;
using Car_Mender.Domain.Entities;

namespace Car_Mender.Domain.Repositories;

public interface IAppointmentRepository
{
	Task<Result<Appointment>> GetAppointmentByIdAsync(Guid id);
	Task<IEnumerable<Appointment>> GetAllAppointmentsAsync();
	Task AddAppointmentAsync(Appointment appointment);
	Task UpdateAppointmentAsync(Appointment appointment);
	Task DeleteAppointmentAsync(Guid id);
}