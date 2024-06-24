using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Companies.Entities;

namespace Car_Mender.Domain.Features.Companies.Repository;

public interface ICompanyRepository
{
    Task<Result<Company>> GetCompanyByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Result<Company>> GetCompanyByIdAsNoTrackingAsync(Guid id, CancellationToken cancellationToken);
    Task<Result<IEnumerable<Company>>> GetAllCompaniesAsync();
    Task<Result<Guid>> CreateCompanyAsync(Company company, CancellationToken cancellationToken);
    Task<Result> UpdateCompanyAsync(Company company);
    Task<Result> DeleteCompanyAsync(Guid id);
}