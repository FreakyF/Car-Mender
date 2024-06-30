using AutoMapper;
using Car_Mender.Infrastructure.Features.Appointments.Mapping;

namespace Car_Mender.Infrastructure.Tests.Features.Branches.Mapping;

public class AppointmentMappingProfileTests
{
	private readonly MapperConfiguration _configuration;

	public AppointmentMappingProfileTests()
	{
		_configuration = new MapperConfiguration(cfg => cfg.AddProfile<AppointmentMappingProfile>());
	}

	[Fact]
	public void Profile_ShouldBeValid()
	{
		_configuration.AssertConfigurationIsValid();
	}
}