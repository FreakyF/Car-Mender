using Car_Mender.Domain.Models;

namespace Car_Mender.Domain.Features.Branches.DTOs;

public class UpdateBranchDto
{
	public required string Name { get; init; }
	public required Address Address { get; init; }
	public required string Email { get; init; }
	public required string Phone { get; init; }
}