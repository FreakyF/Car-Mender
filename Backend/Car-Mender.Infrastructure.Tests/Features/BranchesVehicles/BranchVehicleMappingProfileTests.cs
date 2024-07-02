using AutoMapper;
using Car_Mender.Infrastructure.Features.BranchesVehicles.Mapping;

namespace Car_Mender.Infrastructure.Tests.Features.Branches.Mapping;

public class BranchVehicleMappingProfileTests
{
	private readonly MapperConfiguration _configuration;

	public BranchVehicleMappingProfileTests()
	{
		_configuration = new MapperConfiguration(cfg => cfg.AddProfile<BranchVehicleMappingProfile>());
	}

	[Fact]
	public void Profile_ShouldBeValid()
	{
		_configuration.AssertConfigurationIsValid();
	}
}