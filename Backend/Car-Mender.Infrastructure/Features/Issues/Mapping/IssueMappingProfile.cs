using AutoMapper;
using Car_Mender.Domain.Features.Issues.DTOs;
using Car_Mender.Domain.Features.Issues.Entities;
using Car_Mender.Infrastructure.Features.Issues.Commands.CreateIssue;

namespace Car_Mender.Infrastructure.Features.Issues.Mapping;

public class IssueMappingProfile : Profile
{
	public IssueMappingProfile()
	{
		CreateMap<CreateIssueCommand, Issue>()
			.ForMember(dest => dest.Id, opt => opt.Ignore())
			.ForMember(dest => dest.Creator, opt => opt.Ignore())
			.ForMember(dest => dest.Appointment, opt => opt.Ignore());
		CreateMap<Issue, GetIssueDto>();
		CreateMap<Issue, UpdateIssueDto>();
	}
}