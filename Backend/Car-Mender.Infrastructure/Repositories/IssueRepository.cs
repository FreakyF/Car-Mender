using Car_Mender.Domain.Entities;
using Car_Mender.Domain.Repositories;
using Car_Mender.Infrastructure.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace Car_Mender.Infrastructure.Repositories;

public class IssueRepository(AppDbContext context) : IIssueRepository
{
    public async Task<Issue> GetIssueByIdAsync(Guid id)
    {
        return await context.Set<Issue>().FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<IEnumerable<Issue>> GetAllIssuesAsync()
    {
        return await context.Set<Issue>().ToListAsync();
    }

    public async Task AddIssueAsync(Issue issue)
    {
        await context.Set<Issue>().AddAsync(issue);
        await context.SaveChangesAsync();
    }

    public async Task UpdateIssueAsync(Issue issue)
    {
        context.Set<Issue>().Update(issue);
        await context.SaveChangesAsync();
    }

    public async Task DeleteIssueAsync(Guid id)
    {
        var issue = await GetIssueByIdAsync(id);
        context.Set<Issue>().Remove(issue);
        await context.SaveChangesAsync();
    }
}