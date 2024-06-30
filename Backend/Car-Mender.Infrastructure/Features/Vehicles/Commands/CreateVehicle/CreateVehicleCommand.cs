using Car_Mender.Domain.Common;
using MediatR;

namespace Car_Mender.Infrastructure.Features.Vehicles.Commands.CreateVehicle;

// ReSharper disable once ClassNeverInstantiated.Global
public class CreateVehicleCommand : IRequest<Result<Guid>>
{
	public Guid EngineId { get; set; }
	public required string Vin { get; set; }
	public required string Make { get; set; }
	public required string Model { get; set; }
	public required string Generation { get; set; }
	public required uint Year { get; set; }
}