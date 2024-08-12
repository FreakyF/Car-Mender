using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Engines.Entities;

namespace Car_Mender.Domain.Features.Engines.Repository;

public interface IEngineRepository
{
	Task<Result<Engine>> GetEngineByIdAsync(Guid id, CancellationToken cancellationToken);
	Task<Result<Engine>> GetEngineByIdNoTrackingAsync(Guid id, CancellationToken cancellationToken);
	Task<Result<IEnumerable<Engine>>> GetAllEnginesAsync(CancellationToken cancellationToken);
	Task<Result<Guid>> CreateEngineAsync(Engine engine, CancellationToken cancellationToken);
	Task<Result> DeleteEngineAsync(Guid id, CancellationToken cancellationToken);
	Task<Result<bool>> ExistsAsync(Guid id);
	Task<Result<int>> SaveChangesAsync(CancellationToken cancellationToken);
}