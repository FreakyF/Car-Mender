using AutoMapper;
using Car_Mender.Domain.Features.Companies.DTOs;
using Car_Mender.Domain.Features.Companies.Entities;
using Car_Mender.Infrastructure.Features.Companies.Commands.CreateCompany;

namespace Car_Mender.Infrastructure.Features.Companies.Mapping;

public class CompanyMappingProfile : Profile
{
	public CompanyMappingProfile()
	{
		CreateMap<CreateCompanyCommand, Company>()
			.ForMember(dest => dest.Id, opt => opt.Ignore());
		CreateMap<Company, GetCompanyDto>();
		CreateMap<Company, UpdateCompanyDto>();
	}
}