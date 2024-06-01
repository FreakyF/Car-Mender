using Car_Mender.Domain.Entities;
using Car_Mender.Domain.Repositories;
using Car_Mender.Infrastructure.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace Car_Mender.Infrastructure.Repositories;

public class WorkerRepository(AppDbContext context) : IWorkerRepository
{
    public async Task<Worker> GetWorkerByIdAsync(Guid id)
    {
        return await context.Set<Worker>().FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<IEnumerable<Worker>> GetAllWorkersAsync()
    {
        return await context.Set<Worker>().ToListAsync();
    }

    public async Task AddWorkerAsync(Worker worker)
    {
        await context.Set<Worker>().AddAsync(worker);
        await context.SaveChangesAsync();
    }

    public async Task UpdateWorkerAsync(Worker worker)
    {
        context.Set<Worker>().Update(worker);
        await context.SaveChangesAsync();
    }

    public async Task DeleteWorkerAsync(Guid id)
    {
        var worker = await GetWorkerByIdAsync(id);
        context.Set<Worker>().Remove(worker);
        await context.SaveChangesAsync();
    }
}