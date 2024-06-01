using Car_Mender.Domain.Common;
using Car_Mender.Domain.Entities;
using Car_Mender.Domain.Repositories;
using Car_Mender.Infrastructure.Persistence.DatabaseContext;

namespace Car_Mender.Infrastructure.Repositories;

public class AppointmentRepository(AppDbContext context) : IAppointmentRepository
{
    public Task<Result<Appointment>> GetAppointmentByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Appointment>> GetAllAppointmentsAsync()
    {
        throw new NotImplementedException();
    }

    public Task AddAppointmentAsync(Appointment appointment)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAppointmentAsync(Appointment appointment)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAppointmentAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}