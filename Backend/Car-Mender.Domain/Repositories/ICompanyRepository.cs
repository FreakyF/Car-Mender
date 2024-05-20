using Car_Mender.Domain.Entities;

namespace Car_Mender.Domain.Repositories;

public interface ICompanyRepository
{
    Task<Company> GetCompanyByIdAsync(Guid id);
    Task<IEnumerable<Company>> GetAllCompaniesAsync();
    Task AddCompanyAsync(Company company);
    Task UpdateCompanyAsync(Company company);
    Task DeleteCompanyAsync(Guid id);
}