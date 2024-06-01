using Car_Mender.Domain.Entities;
using Car_Mender.Domain.Repositories;
using Car_Mender.Infrastructure.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace Car_Mender.Infrastructure.Repositories;

public class EngineRepository(AppDbContext context) : IEngineRepository
{
    public async Task<Engine> GetEngineByIdAsync(Guid id)
    {
        return await context.Set<Engine>().FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<IEnumerable<Engine>> GetAllEnginesAsync()
    {
        return await context.Set<Engine>().ToListAsync();
    }

    public async Task AddEngineAsync(Engine engine)
    {
        await context.Set<Engine>().AddAsync(engine);
        await context.SaveChangesAsync();
    }

    public async Task UpdateEngineAsync(Engine engine)
    {
        context.Set<Engine>().Update(engine);
        await context.SaveChangesAsync();
    }

    public async Task DeleteEngineAsync(Guid id)
    {
        var engine = await GetEngineByIdAsync(id);
        context.Set<Engine>().Remove(engine);
        await context.SaveChangesAsync();
    }
}