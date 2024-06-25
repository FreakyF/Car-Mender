using AutoMapper;
using Car_Mender.API.Features.Swagger;

namespace Car_Mender.API.Tests.Core.Mapping;

public class JsonPatchMappingProfileTests
{
	private readonly MapperConfiguration _configuration;

	public JsonPatchMappingProfileTests()
	{
		_configuration = new MapperConfiguration(cfg => cfg.AddProfile<JsonPatchMappingProfile>());
	}

	[Fact]
	public void Profile_ShouldBeValid()
	{
		_configuration.AssertConfigurationIsValid();
	}
}