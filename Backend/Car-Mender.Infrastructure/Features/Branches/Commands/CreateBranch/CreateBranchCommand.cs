using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Companies.Entities;
using Car_Mender.Domain.Models;
using MediatR;

namespace Car_Mender.Infrastructure.Features.Branches.Commands.CreateBranch;

// ReSharper disable once ClassNeverInstantiated.Global
public class CreateBranchCommand : IRequest<Result<Guid>>
{
	public Guid CompanyId { get; set; }
	public required string Name { get; set; }
	public required Address Address { get; set; }
	public required string Email { get; set; }
	public required string Phone { get; set; }
}