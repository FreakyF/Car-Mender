using System.Text;
using System.Text.Json;
using Car_Mender.Domain.Enums;
using Car_Mender.Domain.Features.Branches.Entities;
using Car_Mender.Domain.Features.Companies.Entities;
using Car_Mender.Domain.Features.Engines.Entities;
using Car_Mender.Domain.Features.Vehicles.Entities;
using Car_Mender.Domain.Models;
using Car_Mender.Infrastructure.Features.BranchesVehicles.Commands.CreateBranchVehicle;
using Car_Mender.Infrastructure.Persistence.DatabaseContext;

namespace Car_Mender.API.IntegrationTests.Features.BranchesVehicles;

public class BranchVehicleControllerTests : BaseIntegrationTest, IDisposable
{
	private readonly IServiceProvider _services;
	private readonly IServiceScope _scope;

	public BranchVehicleControllerTests(IntegrationTestWebAppFactory factory) : base(factory)
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
	public async Task CreateBranchVehicleAsync_ShouldReturnSuccessfulStatusCode()
	{
		// Arrange
		var dbContext = _services.GetRequiredService<AppDbContext>();

		var company = new Company
		{
			Email = "test@mail.com",
			Name = "TestName",
			Address = new Address
			{
				Street = "TestStreet",
				City = "TestCity",
				PostalCode = "12-345",
				Region = "TestRegion",
				Country = "TestCountry"
			},
			Phone = "123456789",
			Nip = "123-456-78-90"
		};

		await dbContext.Companies.AddAsync(company);

		var branch = new Branch
		{
			Company = company,
			CompanyId = company.Id,
			Name = "TestBranch",
			Address = new Address
			{
				Street = "TestStreet",
				City = "TestCity",
				PostalCode = "12-345",
				Region = "TestRegion",
				Country = "TestCountry"
			},
			Email = "test@mail.com",
			Phone = "123456789",
		};

		await dbContext.Branches.AddAsync(branch);

		var engine = new Engine
		{
			EngineCode = "TestCode",
			Displacement = 1600,
			PowerKw = 78,
			TorqueNm = 120,
			FuelType = FuelType.Gasoline
		};

		await dbContext.Engines.AddAsync(engine);

		var vehicle = new Vehicle
		{
			Vin = "TestVin1234567890",
			Make = "TestMake",
			Model = "TestModel",
			Generation = "TestGeneration",
			Year = 2024,
			EngineId = engine.Id
		};


		await dbContext.Vehicles.AddAsync(vehicle);

		await dbContext.SaveChangesAsync();

		var command = new CreateBranchVehicleCommand
		{
			VehicleId = vehicle.Id,
			BranchId = branch.Id
		};

		var json = JsonSerializer.Serialize(command);
		var content = new StringContent(json, Encoding.UTF8, "application/json");

		// Act
		var response = await Client.PostAsync("api/branchVehicle", content);

		// Assert
		Assert.True(response.IsSuccessStatusCode);
	}
}