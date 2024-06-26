using AutoMapper;
using Car_Mender.Infrastructure.Features.Branches.Mapping;

namespace Car_Mender.Infrastructure.Tests.Features.Branches.Mapping;

public class BranchMappingProfileTests
{
	private readonly MapperConfiguration _configuration;

	public BranchMappingProfileTests()
	{
		_configuration = new MapperConfiguration(cfg => cfg.AddProfile<BranchMappingProfile>());
	}

	[Fact]
	public void Profile_ShouldBeValid()
	{
		_configuration.AssertConfigurationIsValid();
	}
}