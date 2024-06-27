using AutoMapper;
using Car_Mender.Infrastructure.Features.Workers.Mapping;

namespace Car_Mender.Infrastructure.Tests.Features.Workers.Mapping;

public class WorkerMappingProfileTests
{
	private readonly MapperConfiguration _configuration;

	public WorkerMappingProfileTests()
	{
		_configuration = new MapperConfiguration(cfg => cfg.AddProfile<WorkerMappingProfile>());
	}

	[Fact]
	public void Profile_ShouldBeValid()
	{
		_configuration.AssertConfigurationIsValid();
	}
}