using AutoMapper;
using Car_Mender.Infrastructure.Features.Engines.Mapping;

namespace Car_Mender.Infrastructure.Tests.Features.Engines.Mapping;

public class EngineMappingProfileTests
{
	private readonly MapperConfiguration _configuration;

	public EngineMappingProfileTests()
	{
		_configuration = new MapperConfiguration(cfg => cfg.AddProfile<EngineMappingProfile>());
	}

	[Fact]
	public void Profile_ShouldBeValid()
	{
		_configuration.AssertConfigurationIsValid();
	}
}