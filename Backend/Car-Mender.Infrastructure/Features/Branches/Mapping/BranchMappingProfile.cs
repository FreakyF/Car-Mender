using AutoMapper;
using Car_Mender.Domain.Features.Branches.DTOs;
using Car_Mender.Domain.Features.Branches.Entities;
using Car_Mender.Infrastructure.Features.Branches.Commands.CreateBranch;

namespace Car_Mender.Infrastructure.Features.Branches.Mapping;

public class BranchMappingProfile : Profile
{
	public BranchMappingProfile()
	{
		CreateMap<CreateBranchCommand, Branch>()
			.ForMember(dest => dest.Id, opt => opt.Ignore())
			.ForMember(dest => dest.Company, opt => opt.Ignore());
		CreateMap<Branch, GetBranchDto>();
		CreateMap<Branch, UpdateBranchDto>();
	}
}