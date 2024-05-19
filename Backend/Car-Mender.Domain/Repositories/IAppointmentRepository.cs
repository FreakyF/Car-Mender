using Car_Mender.Domain.Entities;

namespace Car_Mender.Domain.Repositories;

public interface IAppointmentRepository
{
    Task<Appointment> GetAppointmentByIdAsync(Guid id);
}