using Car_Mender.Domain.Common;
using Car_Mender.Domain.Entities;
using Car_Mender.Domain.Features.Issues.Entities;

namespace Car_Mender.Domain.Features.Issues.Repository;

public interface IIssueRepository
{
	Task<Result<Issue>> GetIssueByIdAsync(Guid id, CancellationToken cancellationToken);
	Task<Result<Issue>> GetIssueByIdNoTrackingAsync(Guid id, CancellationToken cancellationToken);
	Task<Result<IEnumerable<Issue>>> GetAllIssuesAsync(Guid vehicleId, CancellationToken cancellationToken);
	Task<Result<Guid>> CreateIssueAsync(Issue issue, CancellationToken cancellationToken);
	Task<Result> DeleteIssueAsync(Guid id, CancellationToken cancellationToken);
	Task<Result<bool>> ExistsAsync(Guid id);
	Task<Result<int>> SaveChangesAsync(CancellationToken cancellationToken);
}