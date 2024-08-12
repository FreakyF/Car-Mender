using AutoMapper;
using Car_Mender.Domain.Features.Companies.DTOs;
using Car_Mender.Domain.TestTools.Features.Companies.Entities;
using Car_Mender.Infrastructure.Features.Companies.Mapping;

namespace Car_Mender.Infrastructure.Tests.Features.Companies.Mapping;

public class CompanyMappingProfileTests
{
	private readonly MapperConfiguration _configuration;
	private readonly IMapper _mapper;

	public CompanyMappingProfileTests()
	{
		_configuration = new MapperConfiguration(cfg => cfg.AddProfile<CompanyMappingProfile>());
		_mapper = _configuration.CreateMapper();
	}

	[Fact]
	public void Profile_ShouldBeValid()
	{
		_configuration.AssertConfigurationIsValid();
	}

	[Fact]
	public void Map_WhenTargetTypeIsGetCompanyDto_HasExpectedValues()
	{
		// Arrange
		var company = CompanyFactory.Create();

		// Act
		var getCompanyDto = _mapper.Map<GetCompanyDto>(company);

		// Assert
		getCompanyDto.AssertEqualTo(company);
	}
}