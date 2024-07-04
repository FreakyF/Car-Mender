using System.Text;
using System.Text.Json;
using Car_Mender.Domain.Enums;
using Car_Mender.Infrastructure.Features.Engines.Commands.CreateEngine;

namespace Car_Mender.API.IntegrationTests.Features.Engines;

public class EngineControllerTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
	[Fact]
	public async Task CreateEngineAsync_ShouldReturnSuccessfulStatusCode()
	{
		// Arrange
		var command = new CreateEngineCommand
		{
			EngineCode = "TestCode",
			Displacement = 1600,
			PowerKw = 78,
			TorqueNm = 120,
			FuelType = FuelType.Gasoline
		};

		var json = JsonSerializer.Serialize(command);
		var content = new StringContent(json, Encoding.UTF8, "application/json");

		// Act
		var response = await Client.PostAsync("api/engine", content);

		// Assert
		Assert.True(response.IsSuccessStatusCode);
	}
}