using AutoMapper;
using Car_Mender.Domain.Common;
using Car_Mender.Domain.Entities;
using Car_Mender.Domain.Features.BranchesVehicles.DTOs;
using Car_Mender.Infrastructure.Features.BranchesVehicles.Commands.CreateBranchVehicle;
using MediatR;

namespace Car_Mender.Infrastructure.Features.BranchesVehicles.Mapping;

public class BranchVehicleMappingProfile : Profile
{
	public BranchVehicleMappingProfile()
	{
		CreateMap<CreateBranchVehicleCommand, BranchVehicle>()
			.ForMember(dest => dest.Id, opt => opt.Ignore())
			.ForMember(dest => dest.Branch, opt => opt.Ignore())
			.ForMember(dest => dest.Vehicle, opt => opt.Ignore());
		CreateMap<BranchVehicle, GetBranchVehicleDto>();
		CreateMap<BranchVehicle, UpdateBranchVehicleDto>();
	}
}