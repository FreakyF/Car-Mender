using AutoMapper;
using Car_Mender.Infrastructure.Features.Companies.Mapping;

namespace Car_Mender.Infrastructure.Tests.Features.Companies.Mapping;

public class CompanyMappingProfileTests
{
	private readonly MapperConfiguration _configuration;

	public CompanyMappingProfileTests()
	{
		_configuration = new MapperConfiguration(cfg => cfg.AddProfile<CompanyMappingProfile>());
	}

	[Fact]
	public void Profile_ShouldBeValid()
	{
		_configuration.AssertConfigurationIsValid();
	}
}