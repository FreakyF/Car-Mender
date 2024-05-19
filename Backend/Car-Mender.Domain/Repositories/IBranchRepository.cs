using Car_Mender.Domain.Entities;

namespace Car_Mender.Domain.Repositories;

public interface IBranchRepository
{
    Task<Branch> GetBranchByIdAsync(Guid id);
}