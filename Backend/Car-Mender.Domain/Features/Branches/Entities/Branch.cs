using Car_Mender.Domain.Entities;
using Car_Mender.Domain.Features.Companies.Entities;
using Car_Mender.Domain.Models;

namespace Car_Mender.Domain.Features.Branches.Entities;

public class Branch : BaseEntity
{
	public Guid CompanyId { get; init; }
	public required Company Company { get; init; }
	public required string Name { get; set; }
	public required Address Address { get; set; }
	public required string Email { get; set; }
	public required string Phone { get; set; }
}