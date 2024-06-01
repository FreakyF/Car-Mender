using Car_Mender.Domain.Entities;
using Car_Mender.Domain.Repositories;
using Car_Mender.Infrastructure.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace Car_Mender.Infrastructure.Repositories;

public class CompanyRepository(AppDbContext context) : ICompanyRepository
{
    public async Task<Company> GetCompanyByIdAsync(Guid id)
    {
        return await context.Set<Company>().FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<IEnumerable<Company>> GetAllCompaniesAsync()
    {
        return await context.Set<Company>().ToListAsync();
    }

    public async Task AddCompanyAsync(Company company)
    {
        await context.Set<Company>().AddAsync(company);
        await context.SaveChangesAsync();
    }

    public async Task UpdateCompanyAsync(Company company)
    {
        context.Set<Company>().Update(company);
        await context.SaveChangesAsync();
    }

    public async Task DeleteCompanyAsync(Guid id)
    {
        var company = await GetCompanyByIdAsync(id);
        context.Set<Company>().Remove(company);
        await context.SaveChangesAsync();
    }
}