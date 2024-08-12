using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Workers.Entities;

namespace Car_Mender.Domain.Features.Workers.Repository;

public interface IWorkerRepository
{
	Task<Result<Worker>> GetWorkerByIdAsync(Guid id, CancellationToken cancellationToken);
	Task<Result<Worker>> GetWorkerByIdNoTrackingAsync(Guid id, CancellationToken cancellationToken);
	Task<Result<IEnumerable<Worker>>> GetAllWorkersAsync(CancellationToken cancellationToken);
	Task<Result<Guid>> CreateWorkerAsync(Worker worker, CancellationToken cancellationToken);
	Task<Result> UpdateWorkerAsync(Worker worker, CancellationToken cancellationToken);
	Task<Result> DeleteWorkerAsync(Guid id, CancellationToken cancellationToken);
	Task<Result<bool>> ExistsAsync(Guid id);
	Task<Result<int>> SaveChangesAsync(CancellationToken cancellationToken);
}