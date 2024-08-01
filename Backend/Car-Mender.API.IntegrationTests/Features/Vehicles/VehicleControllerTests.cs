using System.Net;
using System.Text;
using System.Text.Json;
using Car_Mender.Domain.Enums;
using Car_Mender.Domain.Features.Branches.DTOs;
using Car_Mender.Domain.Features.Branches.Entities;
using Car_Mender.Domain.Features.Companies.Entities;
using Car_Mender.Domain.Features.Engines.Entities;
using Car_Mender.Domain.Features.Vehicles.DTOs;
using Car_Mender.Domain.Features.Vehicles.Entities;
using Car_Mender.Infrastructure.Features.Branches.Commands.CreateBranch;
using Car_Mender.Infrastructure.Features.Engines.Commands.CreateEngine;
using Car_Mender.Infrastructure.Features.Vehicles.Commands.CreateVehicle;
using Car_Mender.Infrastructure.Persistence.DatabaseContext;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Car_Mender.API.IntegrationTests.Features.Vehicles;

public class VehicleControllerTests : BaseIntegrationTest, IDisposable
{
    private readonly IServiceScope _scope;
    private readonly IAppDbContext _dbContext;

    public VehicleControllerTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
        _scope = Server.Services.CreateScope();
        var provider = _scope.ServiceProvider;
        _dbContext = provider.GetRequiredService<IAppDbContext>();
    }

    [Fact]
    public async Task CreateVehicleAsync_ValidVehicle_ShouldReturnSuccessfulStatusCode()
    {
        // Arrange
        var engine = new Engine
        {
            EngineCode = "Test-Code-123",
            Displacement = 123,
            PowerKw = 123,
            TorqueNm = 123,
            FuelType = FuelType.Gasoline
        };

        await _dbContext.Engines.AddAsync(engine);
        await _dbContext.SaveChangesAsync();

        var command = new CreateVehicleCommand
        {
            EngineId = engine.Id,
            Vin = "1C3CDFBBXFD224921",
            Make = "TestMake",
            Model = "TestModel",
            Generation = "TestGeneration",
            Year = 2024
        };

        var json = JsonSerializer.Serialize(command);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await Client.PostAsync("api/vehicle", content);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task CreateVehicleAsync_InvalidVehicle_ShouldReturnBadRequestStatusCode()
    {
        // Arrange
        var engine = new Engine
        {
            EngineCode = "Test-Code-123",
            Displacement = 123,
            PowerKw = 123,
            TorqueNm = 123,
            FuelType = FuelType.Gasoline
        };

        await _dbContext.Engines.AddAsync(engine);
        await _dbContext.SaveChangesAsync();

        var command = new CreateVehicleCommand
        {
            EngineId = engine.Id,
            Vin = "",
            Make = "TestMake",
            Model = "TestModel",
            Generation = "TestGeneration",
            Year = 2024
        };

        var json = JsonSerializer.Serialize(command);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await Client.PostAsync("api/vehicle", content);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetVehicleAsync_ValidId_ShouldReturnValidVehicle()
    {
        // Arrange
        var engine = new Engine
        {
            EngineCode = "Test-Code-123",
            Displacement = 123,
            PowerKw = 123,
            TorqueNm = 123,
            FuelType = FuelType.Gasoline
        };

        await _dbContext.Engines.AddAsync(engine);
        await _dbContext.SaveChangesAsync();

        var vehicle = new Vehicle
        {
            EngineId = engine.Id,
            Vin = "",
            Make = "TestMake",
            Model = "TestModel",
            Generation = "TestGeneration",
            Year = 2024
        };

        await _dbContext.Vehicles.AddAsync(vehicle);
        await _dbContext.SaveChangesAsync();

        // Act
        var response = await Client.GetAsync($"api/vehicle/{vehicle.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetVehicleAsync_InvalidId_ShouldReturnNotFoundStatusCode()
    {
        // Arrange
        var engine = new Engine
        {
            EngineCode = "Test-Code-123",
            Displacement = 123,
            PowerKw = 123,
            TorqueNm = 123,
            FuelType = FuelType.Gasoline
        };

        await _dbContext.Engines.AddAsync(engine);
        await _dbContext.SaveChangesAsync();

        var vehicle = new Vehicle
        {
            EngineId = engine.Id,
            Vin = "",
            Make = "TestMake",
            Model = "TestModel",
            Generation = "TestGeneration",
            Year = 2024
        };

        const string invalidId = "9c0287a9-4bdc-4937-a22b-c9d1c7118803";

        await _dbContext.Vehicles.AddAsync(vehicle);
        await _dbContext.SaveChangesAsync();

        // Act
        var response = await Client.GetAsync($"api/vehicle/{invalidId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task UpdateVehicleAsync_ValidId_ShouldReturnNoContentStatusCode()
    {
        // Arrange
        var engine = new Engine
        {
            EngineCode = "Test-Code-123",
            Displacement = 123,
            PowerKw = 123,
            TorqueNm = 123,
            FuelType = FuelType.Gasoline
        };

        var newEngine = new Engine
        {
            EngineCode = "Test-Code-123",
            Displacement = 123,
            PowerKw = 123,
            TorqueNm = 123,
            FuelType = FuelType.Gasoline
        };

        await _dbContext.Engines.AddAsync(engine);
        await _dbContext.SaveChangesAsync();

        var vehicle = new Vehicle
        {
            EngineId = engine.Id,
            Vin = "",
            Make = "TestMake",
            Model = "TestModel",
            Generation = "TestGeneration",
            Year = 2024
        };

        // Act
        var patchDoc = new JsonPatchDocument<UpdateVehicleDto>();
        patchDoc.Replace(c => c.EngineId, newEngine.Id);

        var serializedPatchDoc = JsonConvert.SerializeObject(patchDoc);
        var content = new StringContent(serializedPatchDoc, Encoding.UTF8, "application/json");
        var response = await Client.PatchAsync($"api/vehicle/{vehicle.Id}", content);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task UpdateVehicleAsync_InvalidId_ShouldReturnNotFoundStatusCode()
    {
        // Arrange
        var engine = new Engine
        {
            EngineCode = "Test-Code-123",
            Displacement = 123,
            PowerKw = 123,
            TorqueNm = 123,
            FuelType = FuelType.Gasoline
        };

        var newEngine = new Engine
        {
            EngineCode = "Test-Code-123",
            Displacement = 123,
            PowerKw = 123,
            TorqueNm = 123,
            FuelType = FuelType.Gasoline
        };

        const string invalidId = "9c0287a9-4bdc-4937-a22b-c9d1c7118803";

        await _dbContext.Engines.AddAsync(engine);
        await _dbContext.SaveChangesAsync();

        var vehicle = new Vehicle
        {
            EngineId = engine.Id,
            Vin = "",
            Make = "TestMake",
            Model = "TestModel",
            Generation = "TestGeneration",
            Year = 2024
        };

        // Act
        var patchDoc = new JsonPatchDocument<UpdateVehicleDto>();
        patchDoc.Replace(c => c.EngineId, newEngine.Id);

        var serializedPatchDoc = JsonConvert.SerializeObject(patchDoc);
        var content = new StringContent(serializedPatchDoc, Encoding.UTF8, "application/json");
        var response = await Client.PatchAsync($"api/vehicle/{invalidId}", content);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task DeleteVehicleAsync_ValidId_ShouldReturnNoContentStatusCode()
    {
        var engine = new Engine
        {
            EngineCode = "Test-Code-123",
            Displacement = 123,
            PowerKw = 123,
            TorqueNm = 123,
            FuelType = FuelType.Gasoline
        };

        await _dbContext.Engines.AddAsync(engine);
        await _dbContext.SaveChangesAsync();

        var vehicle = new Vehicle
        {
            EngineId = engine.Id,
            Vin = "",
            Make = "TestMake",
            Model = "TestModel",
            Generation = "TestGeneration",
            Year = 2024
        };

        await _dbContext.Vehicles.AddAsync(vehicle);
        await _dbContext.SaveChangesAsync();

        // Act
        var response = await Client.DeleteAsync($"api/vehicle/{vehicle.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task DeleteVehicleAsync_InvalidId_ShouldReturnNoContentStatusCode()
    {
        // Arrange
        var engine = new Engine
        {
            EngineCode = "Test-Code-123",
            Displacement = 123,
            PowerKw = 123,
            TorqueNm = 123,
            FuelType = FuelType.Gasoline
        };

        await _dbContext.Engines.AddAsync(engine);
        await _dbContext.SaveChangesAsync();

        var vehicle = new Vehicle
        {
            EngineId = engine.Id,
            Vin = "",
            Make = "TestMake",
            Model = "TestModel",
            Generation = "TestGeneration",
            Year = 2024
        };

        const string invalidId = "9c0287a9-4bdc-4937-a22b-c9d1c7118803";

        await _dbContext.Vehicles.AddAsync(vehicle);
        await _dbContext.SaveChangesAsync();

        // Act
        var response = await Client.DeleteAsync($"api/vehicle/{invalidId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    public new void Dispose()
    {
        _scope.Dispose();
        _dbContext.Dispose();
        base.Dispose();
    }
}