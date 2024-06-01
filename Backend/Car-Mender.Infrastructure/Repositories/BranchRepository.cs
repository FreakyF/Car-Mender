using Car_Mender.Domain.Entities;
using Car_Mender.Domain.Repositories;
using Car_Mender.Infrastructure.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace Car_Mender.Infrastructure.Repositories;

public class BranchRepository(AppDbContext context) : IBranchRepository
{
    public async Task<Branch> GetBranchByIdAsync(Guid id)
    {
        return await context.Set<Branch>().FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<IEnumerable<Branch>> GetAllBranchesAsync()
    {
        return await context.Set<Branch>().ToListAsync();
    }

    public async Task AddBranchAsync(Branch branch)
    {
        await context.Set<Branch>().AddAsync(branch);
        await context.SaveChangesAsync();
    }

    public async Task UpdateBranchAsync(Branch branch)
    {
        context.Set<Branch>().Update(branch);
        await context.SaveChangesAsync();
    }

    public async Task DeleteBranchAsync(Guid id)
    {
        var branch = await GetBranchByIdAsync(id);
        context.Set<Branch>().Remove(branch);
        await context.SaveChangesAsync();
    }
}