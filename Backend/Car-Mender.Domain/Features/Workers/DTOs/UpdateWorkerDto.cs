namespace Car_Mender.Domain.Features.Workers.DTOs;

public class UpdateWorkerDto
{
	public Guid BranchId { get; init; }
	public required string Email { get; init; }
	public required string Password { get; init; }
	public required string FirstName { get; init; }
	public required string LastName { get; init; }
	public required string Phone { get; init; }
}