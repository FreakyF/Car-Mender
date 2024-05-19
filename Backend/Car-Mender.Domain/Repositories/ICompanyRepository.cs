using Car_Mender.Domain.Entities;

namespace Car_Mender.Domain.Repositories;

public interface ICompanyRepository
{
    Task<Company> GetCompanyByIdAsync(Guid id);
}