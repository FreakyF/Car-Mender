using Car_Mender.Domain.Common;
using MediatR;

namespace Car_Mender.Infrastructure.Features.Workers.Commands.CreateWorker;

// ReSharper disable once ClassNeverInstantiated.Global
public class CreateWorkerCommand : IRequest<Result<Guid>>
{
	public Guid BranchId { get; set; }
	public required string Email { get; set; }
	public required string Password { get; set; }
	public required string FirstName { get; set; }
	public required string LastName { get; set; }
	public required string Phone { get; set; }
}