using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Issues.Entities;
using Car_Mender.Domain.Features.Issues.Errors;
using Car_Mender.Domain.Features.Issues.Repository;
using Car_Mender.Domain.Features.Workers.Entities;
using Car_Mender.Domain.Features.Workers.Errors;
using Car_Mender.Infrastructure.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace Car_Mender.Infrastructure.Features.Issues.Repository;

public class IssueRepository(AppDbContext context) : IIssueRepository
{
	public async Task<Result<Issue>> GetIssueByIdAsync(Guid id, CancellationToken cancellationToken)
	{
		var issue = await context.Issues.FirstOrDefaultAsync(w => w.Id == id, cancellationToken);

		return issue is null
			? IssueErrors.CouldNotBeFound
			: Result<Issue>.Success(issue);
	}

	public async Task<Result<Issue>> GetIssueByIdNoTrackingAsync(Guid id, CancellationToken cancellationToken)
	{
		var issue = await context.Issues
			.AsNoTracking()
			.FirstOrDefaultAsync(w => w.Id == id, cancellationToken);

		return issue is null
			? IssueErrors.CouldNotBeFound
			: Result<Issue>.Success(issue);
	}

	public async Task<Result<IEnumerable<Issue>>> GetAllIssuesAsync(Guid vehicleId, CancellationToken cancellationToken)
	{
		var issues = await context.Issues
			.Join(context.Appointments,
				issue => issue.AppointmentId,
				appointment => appointment.Id,
				(issue, appointment) => new { issue, appointment })
			.Join(context.Vehicles,
				issueAppointment => issueAppointment.appointment.VehicleId,
				vehicle => vehicle.Id,
				(issueAppointment, vehicle) => new { issueAppointment.issue, vehicle })
			.Where(issueVehicle => issueVehicle.vehicle.Id == vehicleId)
			.Select(issueVehicle => issueVehicle.issue)
			.ToListAsync(cancellationToken);

		return issues.Count == 0
			? IssueErrors.CouldNotBeFound
			: Result<IEnumerable<Issue>>.Success(issues);
	}

	public async Task<Result<Guid>> CreateIssueAsync(Issue issue, CancellationToken cancellationToken)
	{
		try
		{
			await context.Issues.AddAsync(issue, cancellationToken);
			await context.SaveChangesAsync(cancellationToken);
		}
		catch (Exception)
		{
			return Error.Unexpected;
		}

		return Result<Guid>.Success(issue.Id);
	}

	public async Task<Result> DeleteIssueAsync(Guid id, CancellationToken cancellationToken)
	{
		var getIssueResult = await GetIssueByIdAsync(id, cancellationToken);
		if (getIssueResult.IsFailure) return Result.Failure(getIssueResult.Error);

		var issue = getIssueResult.Value;
		context.Issues.Remove(issue!);
		await context.SaveChangesAsync(cancellationToken);
		return Result.Success();
	}

	public async Task<Result<bool>> ExistsAsync(Guid id)
	{
		var exists = await context.Issues.AnyAsync(i => i.Id == id);
		return Result<bool>.Success(exists);
	}

	public async Task<Result<int>> SaveChangesAsync(CancellationToken cancellationToken)
	{
		var writtenEntities = await context.SaveChangesAsync(cancellationToken);
		return Result<int>.Success(writtenEntities);
	}
}