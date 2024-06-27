using AutoMapper;
using Car_Mender.Domain.Features.Engines.DTOs;
using Car_Mender.Domain.Features.Engines.Entities;
using Car_Mender.Infrastructure.Features.Engines.Commands.CreateEngine;

namespace Car_Mender.Infrastructure.Features.Engines.Mapping;

public class EngineMappingProfile : Profile
{
	public EngineMappingProfile()
	{
		CreateMap<CreateEngineCommand, Engine>()
			.ForMember(dest => dest.Id, opt => opt.Ignore());
		CreateMap<Engine, GetEngineDto>();
		CreateMap<Engine, UpdateEngineDto>();
	}
}