using AutoMapper;
using Car_Mender.Domain.Features.Engines.DTOs;
using Car_Mender.Domain.Features.Engines.Entities;
using Car_Mender.Domain.Features.Vehicles.DTOs;
using Car_Mender.Domain.Features.Vehicles.Entities;
using Car_Mender.Infrastructure.Features.Vehicles.Commands.CreateVehicle;

namespace Car_Mender.Infrastructure.Features.Vehicles.Mapping;

public class VehicleMappingProfile : Profile
{
	public VehicleMappingProfile()
	{
		CreateMap<CreateVehicleCommand, Vehicle>()
			.ForMember(dest => dest.Id, opt => opt.Ignore())
			.ForMember(dest => dest.Engine, opt => opt.Ignore());
		CreateMap<Vehicle, GetVehicleDto>();
		CreateMap<Vehicle, UpdateVehicleDto>();
	}
}