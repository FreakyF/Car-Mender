using AutoMapper;
using Car_Mender.Infrastructure.Features.Issues.Mapping;

namespace Car_Mender.Infrastructure.Tests.Features.Issues;

public class IssueMappingProfileTests
{
	private readonly MapperConfiguration _configuration;

	public IssueMappingProfileTests()
	{
		_configuration = new MapperConfiguration(cfg => cfg.AddProfile<IssueMappingProfile>());
	}

	[Fact]
	public void Profile_ShouldBeValid()
	{
		_configuration.AssertConfigurationIsValid();
	}
}