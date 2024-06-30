using AutoMapper;
using Car_Mender.Infrastructure.Features.Vehicles.Mapping;

namespace Car_Mender.Infrastructure.Tests.Features.Vehicles;

public class VehicleMappingProfileTests
{
	private readonly MapperConfiguration _configuration;

	public VehicleMappingProfileTests()
	{
		_configuration = new MapperConfiguration(cfg => cfg.AddProfile<VehicleMappingProfile>());
	}

	[Fact]
	public void Profile_ShouldBeValid()
	{
		_configuration.AssertConfigurationIsValid();
	}
}