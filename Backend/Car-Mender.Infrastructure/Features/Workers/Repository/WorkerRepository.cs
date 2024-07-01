using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Workers.Entities;
using Car_Mender.Domain.Features.Workers.Errors;
using Car_Mender.Domain.Features.Workers.Repository;
using Car_Mender.Infrastructure.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace Car_Mender.Infrastructure.Features.Workers.Repository;

public class WorkerRepository(AppDbContext context) : IWorkerRepository
{
	public async Task<Result<Worker>> GetWorkerByIdAsync(Guid id, CancellationToken cancellationToken)
	{
		var worker = await context.Workers.FirstOrDefaultAsync(w => w.Id == id, cancellationToken);

		return worker is null
			? WorkerErrors.CouldNotBeFound
			: Result<Worker>.Success(worker);
	}

	public async Task<Result<Worker>> GetWorkerByIdNoTrackingAsync(Guid id, CancellationToken cancellationToken)
	{
		var worker = await context.Workers
			.AsNoTracking()
			.FirstOrDefaultAsync(w => w.Id == id, cancellationToken);

		return worker is null
			? WorkerErrors.CouldNotBeFound
			: Result<Worker>.Success(worker);
	}

	public Task<Result<IEnumerable<Worker>>> GetAllWorkersAsync()
	{
		throw new NotImplementedException();
	}

	public async Task<Result<Guid>> CreateWorkerAsync(Worker worker, CancellationToken cancellationToken)
	{
		try
		{
			await context.Workers.AddAsync(worker, cancellationToken);
			await context.SaveChangesAsync(cancellationToken);
		}
		catch (Exception)
		{
			return Error.Unexpected;
		}

		return Result<Guid>.Success(worker.Id);
	}

	public Task<Result> UpdateWorkerAsync(Worker worker, CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}

	public async Task<Result> DeleteWorkerAsync(Guid id, CancellationToken cancellationToken)
	{
		var getWorkerResult = await GetWorkerByIdAsync(id, cancellationToken);
		if (getWorkerResult.IsFailure) return Result.Failure(getWorkerResult.Error);

		var worker = getWorkerResult.Value;
		context.Workers.Remove(worker!);
		await context.SaveChangesAsync(cancellationToken);
		return Result.Success();
	}

	public async Task<Result<bool>> ExistsAsync(Guid id)
	{
		var exists = await context.Workers.AnyAsync(w => w.Id == id);
		return Result<bool>.Success(exists);
	}

	public async Task<Result<int>> SaveChangesAsync(CancellationToken cancellationToken)
	{
		var writtenEntities = await context.SaveChangesAsync(cancellationToken);
		return Result<int>.Success(writtenEntities);
	}
}