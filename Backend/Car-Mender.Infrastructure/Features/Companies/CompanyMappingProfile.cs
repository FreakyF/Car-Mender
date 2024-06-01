using AutoMapper;
using Car_Mender.Domain.Features.Companies.DTOs;
using Car_Mender.Domain.Features.Companies.Entities;
using Car_Mender.Infrastructure.Features.Companies.Commands.CreateCompany;

namespace Car_Mender.Infrastructure.Features.Companies;

public class CompanyMappingProfile : Profile
{
	public CompanyMappingProfile()
	{
		CreateMap<CreateCompanyCommand, Company>();
		CreateMap<Company, GetCompanyDto>();
	}
}