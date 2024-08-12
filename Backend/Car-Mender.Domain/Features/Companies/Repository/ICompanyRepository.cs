using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Companies.Entities;

namespace Car_Mender.Domain.Features.Companies.Repository;

public interface ICompanyRepository
{
	Task<Result<Company>> GetCompanyByIdAsync(Guid id, CancellationToken cancellationToken);
	Task<Result<Company>> GetCompanyByIdAsNoTrackingAsync(Guid id, CancellationToken cancellationToken);
	Task<Result<IEnumerable<Company>>> GetAllCompaniesAsync(CancellationToken cancellationToken);
	Task<Result<Guid>> CreateCompanyAsync(Company company, CancellationToken cancellationToken);
	Task<Result> DeleteCompanyAsync(Guid id, CancellationToken cancellationToken);
	Task<Result<bool>> ExistsAsync(Guid id);
	Task<Result<int>> SaveChangesAsync(CancellationToken cancellationToken);
}