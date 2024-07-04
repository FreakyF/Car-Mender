using System.Text;
using System.Text.Json;
using Car_Mender.Domain.Enums;
using Car_Mender.Domain.Features.Engines.Entities;
using Car_Mender.Infrastructure.Features.Vehicles.Commands.CreateVehicle;
using Car_Mender.Infrastructure.Persistence.DatabaseContext;

namespace Car_Mender.API.IntegrationTests.Features.Vehicles;

public class VehicleControllerTests : BaseIntegrationTest, IDisposable
{
	private readonly IServiceProvider _services;
	private readonly IServiceScope _scope;

	public VehicleControllerTests(IntegrationTestWebAppFactory factory) : base(factory)
	{
		_scope = Server.Services.CreateScope();
		_services = _scope.ServiceProvider;
	}

	public new void Dispose()
	{
		_scope.Dispose();
		base.Dispose();
	}

	[Fact]
	public async Task CreateVehicleAsync_ShouldReturnSuccessfulStatusCode()
	{
		// Arrange
		var dbContext = _services.GetRequiredService<AppDbContext>();

		var engine = new Engine
		{
			EngineCode = "TestCode",
			Displacement = 1600,
			PowerKw = 78,
			TorqueNm = 120,
			FuelType = FuelType.Gasoline
		};

		await dbContext.Engines.AddAsync(engine);

		var command = new CreateVehicleCommand
		{
			EngineId = engine.Id,
			Vin = "TestVin1234567890",
			Make = "TestMake",
			Model = "TestModel",
			Generation = "TestGeneration",
			Year = 2024
		};

		await dbContext.SaveChangesAsync();

		var json = JsonSerializer.Serialize(command);
		var content = new StringContent(json, Encoding.UTF8, "application/json");

		// Act
		var response = await Client.PostAsync("api/vehicle", content);

		// Assert
		Assert.True(response.IsSuccessStatusCode);
	}
}