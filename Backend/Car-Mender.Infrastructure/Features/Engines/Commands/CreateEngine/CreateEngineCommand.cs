using Car_Mender.Domain.Common;
using Car_Mender.Domain.Enums;
using MediatR;

namespace Car_Mender.Infrastructure.Features.Engines.Commands.CreateEngine;

// ReSharper disable once ClassNeverInstantiated.Global
public class CreateEngineCommand : IRequest<Result<Guid>>
{
	public required string EngineCode { get; set; }
	public required uint Displacement { get; set; }
	public required uint PowerKw { get; set; }
	public required uint TorqueNm { get; set; }
	public required FuelType FuelType { get; set; }
}