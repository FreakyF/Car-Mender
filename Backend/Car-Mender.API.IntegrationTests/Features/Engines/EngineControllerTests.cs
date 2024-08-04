using System.Net;
using System.Text;
using Car_Mender.Domain.Enums;
using Car_Mender.Domain.Features.Engines.DTOs;
using Car_Mender.Domain.Features.Engines.Entities;
using Car_Mender.Infrastructure.Features.Engines.Commands.CreateEngine;
using Car_Mender.Infrastructure.Persistence.DatabaseContext;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Car_Mender.API.IntegrationTests.Features.Engines;

public class EngineControllerTests : BaseIntegrationTest, IDisposable
{
    private readonly IServiceScope _scope;
    private readonly IAppDbContext _dbContext;

    public EngineControllerTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
        _scope = Server.Services.CreateScope();
        var provider = _scope.ServiceProvider;
        _dbContext = provider.GetRequiredService<IAppDbContext>();
    }

    [Fact]
    public async Task CreateEngineAsync_ValidEngine_ShouldReturnSuccessfulStatusCode()
    {
        // Arrange
        var command = new CreateEngineCommand
        {
            EngineCode = "Test-Code-123",
            Displacement = 123,
            PowerKw = 123,
            TorqueNm = 123,
            FuelType = FuelType.Gasoline
        };

        var json = JsonSerializer.Serialize(command);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await Client.PostAsync("api/engine", content);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task CreateEngineAsync_InvalidEngine_ShouldReturnBadRequestStatusCode()
    {
        // Arrange
        var command = new CreateEngineCommand
        {
            EngineCode = null,
            Displacement = 0,
            PowerKw = 0,
            TorqueNm = 0,
            FuelType = FuelType.Gasoline
        };

        var json = JsonSerializer.Serialize(command);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await Client.PostAsync("api/engine", content);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetEngineAsync_ValidId_ShouldReturnValidEngine()
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

        // Act
        var response = await Client.GetAsync($"api/engine/{engine.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetEngineAsync_InvalidId_ShouldReturnNotFoundStatusCode()
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

        const string invalidId = "9c0287a9-4bdc-4937-a22b-c9d1c7118803";

        await _dbContext.Engines.AddAsync(engine);
        await _dbContext.SaveChangesAsync();

        // Act
        var response = await Client.GetAsync($"api/engine/{invalidId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task UpdateEngineAsync_ValidId_ShouldReturnSuccessfulStatusCode()
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

        var patchDoc = new JsonPatchDocument<UpdateEngineDto>();
        patchDoc.Replace(c => c.EngineCode, "NewTestCode");

        var serializedPatchDoc = JsonConvert.SerializeObject(patchDoc);
        var content = new StringContent(serializedPatchDoc, Encoding.UTF8, "application/json");
        var response = await Client.PatchAsync($"api/engine/{engine.Id}", content);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task UpdateEngineAsync_InvalidId_ShouldReturnSuccessfulStatusCode()
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

        const string invalidId = "9c0287a9-4bdc-4937-a22b-c9d1c7118803";

        await _dbContext.Engines.AddAsync(engine);
        await _dbContext.SaveChangesAsync();

        var patchDoc = new JsonPatchDocument<UpdateEngineDto>();
        patchDoc.Replace(c => c.EngineCode, "NewTestCode");

        var serializedPatchDoc = JsonConvert.SerializeObject(patchDoc);
        var content = new StringContent(serializedPatchDoc, Encoding.UTF8, "application/json");
        var response = await Client.PatchAsync($"api/engine/{invalidId}", content);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeleteEngineAsync_ValidId_ShouldReturnNoContentStatusCode()
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

        // Act
        var response = await Client.DeleteAsync($"api/engine/{engine.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task DeleteEngineAsync_InvalidId_ShouldReturnNoContentStatusCode()
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

        const string invalidId = "9c0287a9-4bdc-4937-a22b-c9d1c7118803";

        await _dbContext.Engines.AddAsync(engine);
        await _dbContext.SaveChangesAsync();

        // Act
        var response = await Client.DeleteAsync($"api/engine/{invalidId}");

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