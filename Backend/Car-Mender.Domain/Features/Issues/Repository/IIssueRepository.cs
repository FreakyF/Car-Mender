using Car_Mender.Domain.Common;
using Car_Mender.Domain.Entities;

namespace Car_Mender.Domain.Features.Issues.Repository;

public interface IIssueRepository
{
	Task<Result<Issue>> GetIssueByIdAsync(Guid id, CancellationToken cancellationToken);
	Task<Result<Issue>> GetIssueByIdNoTrackingAsync(Guid id, CancellationToken cancellationToken);
	Task<IEnumerable<Issue>> GetAllIssuesAsync();
	Task<Result<Guid>> CreateIssueAsync(Issue issue, CancellationToken cancellationToken);
	Task<Result> UpdateIssueAsync(Issue issue, CancellationToken cancellationToken);
	Task<Result> DeleteIssueAsync(Guid id, CancellationToken cancellationToken);
	Task<Result<bool>> ExistsAsync(Guid id);
	Task<Result<int>> SaveChangesAsync(CancellationToken cancellationToken);
}