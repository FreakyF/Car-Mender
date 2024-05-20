using Car_Mender.Domain.Entities;

namespace Car_Mender.Domain.Repositories;

public interface IBranchRepository
{
    Task<Branch> GetBranchByIdAsync(Guid id);
    Task<IEnumerable<Branch>> GetAllBranchesAsync();
    Task AddBranchAsync(Branch branch);
    Task UpdateBranchAsync(Branch branch);
    Task DeleteBranchAsync(Guid id);
}