using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Engines.Entities;
using Car_Mender.Domain.Features.Engines.Errors;
using Car_Mender.Domain.Features.Engines.Repository;
using Car_Mender.Infrastructure.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace Car_Mender.Infrastructure.Features.Engines.Repository;

public class EngineRepository(AppDbContext context) : IEngineRepository
{
	public async Task<Result<Engine>> GetEngineByIdAsync(Guid id, CancellationToken cancellationToken)
	{
		var engine = await context.Engines.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

		return engine is null
			? EngineErrors.CouldNotBeFound
			: Result<Engine>.Success(engine);
	}

	public async Task<Result<Engine>> GetEngineByIdNoTrackingAsync(Guid id, CancellationToken cancellationToken)
	{
		var engine = await context.Engines
			.AsNoTracking()
			.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

		return engine is null
			? EngineErrors.CouldNotBeFound
			: Result<Engine>.Success(engine);
	}

	public async Task<Result<IEnumerable<Engine>>> GetAllEnginesAsync(CancellationToken cancellationToken)
	{
		var engines = await context.Engines.ToListAsync(cancellationToken);

		return engines.Count == 0
			? EngineErrors.CouldNotBeFound
			: Result<IEnumerable<Engine>>.Success(engines);
	}

	public async Task<Result<Guid>> CreateEngineAsync(Engine engine, CancellationToken cancellationToken)
	{
		try
		{
			await context.Engines.AddAsync(engine, cancellationToken);
			await context.SaveChangesAsync(cancellationToken);
		}
		catch (Exception)
		{
			return Error.Unexpected;
		}

		return Result<Guid>.Success(engine.Id);
	}

	public Task<Result> UpdateEngineAsync(Engine engine, CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}

	public async Task<Result> DeleteEngineAsync(Guid id, CancellationToken cancellationToken)
	{
		var getEngineResult = await GetEngineByIdAsync(id, cancellationToken);
		if (getEngineResult.IsFailure) return Result.Failure(getEngineResult.Error);

		var engine = getEngineResult.Value;
		context.Engines.Remove(engine!);
		await context.SaveChangesAsync(cancellationToken);
		return Result.Success();
	}

	public async Task<Result<bool>> ExistsAsync(Guid id)
	{
		var exists = await context.Engines.AnyAsync(e => e.Id == id);
		return Result<bool>.Success(exists);
	}

	public async Task<Result<int>> SaveChangesAsync(CancellationToken cancellationToken)
	{
		var writtenEntities = await context.SaveChangesAsync(cancellationToken);
		return Result<int>.Success(writtenEntities);
	}
}