using Car_Mender.Domain.Entities;

namespace Car_Mender.Domain.Repositories;

public interface IIssueRepository
{
    Task<Issue> GetIssueByIdAsync(Guid id);
    Task<IEnumerable<Issue>> GetAllIssuesAsync();
    Task AddIssueAsync(Issue issue);
    Task UpdateIssueAsync(Issue issue);
    Task DeleteIssueAsync(Guid id);
}