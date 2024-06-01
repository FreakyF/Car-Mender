using Car_Mender.Domain.Common;
using Car_Mender.Domain.Entities;
using Car_Mender.Domain.Features.Companies.DTOs;
using Car_Mender.Domain.Features.Companies.Entities;

namespace Car_Mender.Domain.Features.Companies.Repository;

public interface ICompanyRepository
{
    Task<Result<Company>> GetCompanyByIdAsync(Guid id);
    Task<Result<IEnumerable<Company>>> GetAllCompaniesAsync();
    Task<Result> CreateCompanyAsync(CreateCompanyDto dto, CancellationToken cancellationToken);
    Task<Result> UpdateCompanyAsync(Company company);
    Task<Result> DeleteCompanyAsync(Guid id);
}