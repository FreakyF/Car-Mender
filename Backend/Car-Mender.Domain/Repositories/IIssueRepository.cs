using Car_Mender.Domain.Entities;

namespace Car_Mender.Domain.Repositories;

public interface IIssueRepository
{
    Task<Issue> GetIssueByIdAsync(Guid id);
}