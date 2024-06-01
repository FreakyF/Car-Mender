using Car_Mender.Domain.Entities;
using Car_Mender.Domain.Repositories;
using Car_Mender.Infrastructure.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace Car_Mender.Infrastructure.Repositories;

public class AppointmentRepository(AppDbContext context) : IAppointmentRepository
{
    public async Task<Appointment> GetAppointmentByIdAsync(Guid id)
    {
        return await context.Set<Appointment>().FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<IEnumerable<Appointment>> GetAllAppointmentsAsync()
    {
        return await context.Set<Appointment>().ToListAsync();
    }

    public async Task AddAppointmentAsync(Appointment appointment)
    {
        await context.Set<Appointment>().AddAsync(appointment);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAppointmentAsync(Appointment appointment)
    {
        context.Set<Appointment>().Update(appointment);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAppointmentAsync(Guid id)
    {
        var appointment = await GetAppointmentByIdAsync(id);
        context.Set<Appointment>().Remove(appointment);
        await context.SaveChangesAsync();
    }
}