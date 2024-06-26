using Car_Mender.Domain.Common;
using Car_Mender.Domain.Entities;
using Car_Mender.Domain.Features.Branches.Entities;

namespace Car_Mender.Domain.Features.Branches.Repository;

public interface IBranchRepository
{
	Task<Result<Branch>> GetBranchByIdAsync(Guid id, CancellationToken cancellationToken);
	Task<Result<Branch>> GetBranchByIdNoTrackingAsync(Guid id, CancellationToken cancellationToken);
	Task<Result<IEnumerable<Branch>>> GetAllBranchesAsync();
	Task<Result<Guid>> CreateBranchAsync(Branch branch, CancellationToken cancellationToken);
	Task<Result> UpdateBranchAsync(Branch branch, CancellationToken cancellationToken);
	Task<Result> DeleteBranchAsync(Guid id);
	Task<Result<bool>> ExistsAsync(Guid guid);
	Task<Result<int>> SaveChangesAsync(CancellationToken cancellationToken);
}