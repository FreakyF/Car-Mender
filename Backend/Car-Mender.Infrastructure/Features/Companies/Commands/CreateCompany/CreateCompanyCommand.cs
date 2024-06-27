using Car_Mender.Domain.Common;
using Car_Mender.Domain.Models;
using MediatR;

namespace Car_Mender.Infrastructure.Features.Companies.Commands.CreateCompany;

// ReSharper disable once ClassNeverInstantiated.Global
public record CreateCompanyCommand : IRequest<Result<Guid>>
{
	public required string Email { get; set; }
	public required string Name { get; set; }
	public required Address Address { get; set; }
	public required string Phone { get; set; }
	public required string Nip { get; set; }
}