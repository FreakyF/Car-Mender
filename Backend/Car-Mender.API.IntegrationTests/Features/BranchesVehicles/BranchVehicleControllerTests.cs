using System.Net;
using System.Text;
using Car_Mender.Domain.Entities;
using Car_Mender.Domain.Enums;
using Car_Mender.Domain.Features.Branches.Entities;
using Car_Mender.Domain.Features.BranchesVehicles.DTOs;
using Car_Mender.Domain.Features.Companies.Entities;
using Car_Mender.Domain.Features.Engines.Entities;
using Car_Mender.Domain.Features.Vehicles.Entities;
using Car_Mender.Domain.Models;
using Car_Mender.Infrastructure.Features.BranchesVehicles.Commands.CreateBranchVehicle;
using Car_Mender.Infrastructure.Persistence.DatabaseContext;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Car_Mender.API.IntegrationTests.Features.BranchesVehicles;

public class BranchVehicleControllerTests : BaseIntegrationTest, IDisposable
{
    private readonly IServiceScope _scope;
    private readonly IAppDbContext _dbContext;

    public BranchVehicleControllerTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
        _scope = Server.Services.CreateScope();
        var provider = _scope.ServiceProvider;
        _dbContext = provider.GetRequiredService<IAppDbContext>();
    }

    [Fact]
    public async Task CreateBranchVehicleAsync_ValidBranch_ShouldReturnSuccessfulStatusCode()
    {
        // Arrange
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

        await _dbContext.Companies.AddAsync(company);
        await _dbContext.SaveChangesAsync();

        var branch = new Branch
        {
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

        await _dbContext.Branches.AddAsync(branch);
        await _dbContext.SaveChangesAsync();

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
            Vin = "1C3CDFBBXFD224921",
            Make = "TestMake",
            Model = "TestModel",
            Generation = "TestGeneration",
            Year = 2024
        };

        await _dbContext.Vehicles.AddAsync(vehicle);
        await _dbContext.SaveChangesAsync();

        var command = new CreateBranchVehicleCommand
        {
            BranchId = branch.Id,
            VehicleId = vehicle.Id,
        };

        var json = JsonSerializer.Serialize(command);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await Client.PostAsync("api/branchvehicle", content);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task CreateBranchVehicleAsync_InvalidBranch_ShouldReturnInternalServerErrorStatusCode()
    {
        // Arrange
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

        await _dbContext.Companies.AddAsync(company);
        await _dbContext.SaveChangesAsync();

        var branch = new Branch
        {
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

        await _dbContext.Branches.AddAsync(branch);
        await _dbContext.SaveChangesAsync();

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
            Vin = "1C3CDFBBXFD224921",
            Make = "TestMake",
            Model = "TestModel",
            Generation = "TestGeneration",
            Year = 2024
        };

        await _dbContext.Vehicles.AddAsync(vehicle);
        await _dbContext.SaveChangesAsync();

        var command = new CreateBranchVehicleCommand
        {
            BranchId = Guid.NewGuid(),
            VehicleId = Guid.NewGuid()
        };

        var json = JsonSerializer.Serialize(command);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await Client.PostAsync("api/branchvehicle", content);

        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
    }

    [Fact]
    public async Task GetBranchVehicleAsync_ValidId_ShouldReturnValidBranchVehicle()
    {
        // Arrange
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

        await _dbContext.Companies.AddAsync(company);
        await _dbContext.SaveChangesAsync();

        var branch = new Branch
        {
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

        await _dbContext.Branches.AddAsync(branch);
        await _dbContext.SaveChangesAsync();

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
            Vin = "1C3CDFBBXFD224921",
            Make = "TestMake",
            Model = "TestModel",
            Generation = "TestGeneration",
            Year = 2024
        };

        await _dbContext.Vehicles.AddAsync(vehicle);
        await _dbContext.SaveChangesAsync();

        var branchVehicle = new BranchVehicle
        {
            BranchId = branch.Id,
            VehicleId = vehicle.Id,
        };

        await _dbContext.BranchesVehicles.AddAsync(branchVehicle);
        await _dbContext.SaveChangesAsync();

        // Act
        var response = await Client.GetAsync($"api/branchvehicle/{branchVehicle.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetBranchVehicleAsync_InvalidId_ShouldReturnNotFoundStatusCode()
    {
        // Arrange
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

        await _dbContext.Companies.AddAsync(company);
        await _dbContext.SaveChangesAsync();

        var branch = new Branch
        {
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

        await _dbContext.Branches.AddAsync(branch);
        await _dbContext.SaveChangesAsync();

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
            Vin = "1C3CDFBBXFD224921",
            Make = "TestMake",
            Model = "TestModel",
            Generation = "TestGeneration",
            Year = 2024
        };

        await _dbContext.Vehicles.AddAsync(vehicle);
        await _dbContext.SaveChangesAsync();

        var branchVehicle = new BranchVehicle
        {
            BranchId = branch.Id,
            VehicleId = vehicle.Id,
        };

        await _dbContext.BranchesVehicles.AddAsync(branchVehicle);
        await _dbContext.SaveChangesAsync();

        const string invalidId = "9c0287a9-4bdc-4937-a22b-c9d1c7118803";

        // Act
        var response = await Client.GetAsync($"api/branchvehicle/{invalidId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task UpdateBranchVehicleAsync_ValidId_ShouldReturnInternalServerErrorStatusCode()
    {
        // Arrange
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

        await _dbContext.Companies.AddAsync(company);
        await _dbContext.SaveChangesAsync();

        var branch = new Branch
        {
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

        await _dbContext.Branches.AddAsync(branch);
        await _dbContext.SaveChangesAsync();

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
            Vin = "1C3CDFBBXFD224921",
            Make = "TestMake",
            Model = "TestModel",
            Generation = "TestGeneration",
            Year = 2024
        };

        await _dbContext.Vehicles.AddAsync(vehicle);
        await _dbContext.SaveChangesAsync();


        var newVehicle = new Vehicle
        {
            EngineId = engine.Id,
            Vin = "1C3CDFBBXFD224921",
            Make = "TestMake",
            Model = "TestModel",
            Generation = "TestGeneration",
            Year = 2024
        };

        await _dbContext.Vehicles.AddAsync(newVehicle);
        await _dbContext.SaveChangesAsync();

        var branchVehicle = new BranchVehicle
        {
            BranchId = branch.Id,
            VehicleId = vehicle.Id,
        };

        await _dbContext.BranchesVehicles.AddAsync(branchVehicle);
        await _dbContext.SaveChangesAsync();

        // Act
        var patchDoc = new JsonPatchDocument<UpdateBranchVehicleDto>();
        patchDoc.Replace(c => c.VehicleId, newVehicle.Id);

        var serializedPatchDoc = JsonConvert.SerializeObject(patchDoc);
        var content = new StringContent(serializedPatchDoc, Encoding.UTF8, "application/json");
        var response = await Client.PatchAsync($"api/branchvehicle/{branch.Id}", content);

        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
    }

    [Fact]
    public async Task UpdateBranchVehicleAsync_InvalidId_ShouldReturnInternalServerErrorStatusCode()
    {
        // Arrange
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

        await _dbContext.Companies.AddAsync(company);
        await _dbContext.SaveChangesAsync();

        var branch = new Branch
        {
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

        await _dbContext.Branches.AddAsync(branch);
        await _dbContext.SaveChangesAsync();

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
            Vin = "1C3CDFBBXFD224921",
            Make = "TestMake",
            Model = "TestModel",
            Generation = "TestGeneration",
            Year = 2024
        };

        await _dbContext.Vehicles.AddAsync(vehicle);
        await _dbContext.SaveChangesAsync();


        var newVehicle = new Vehicle
        {
            EngineId = engine.Id,
            Vin = "1C3CDFBBXFD224921",
            Make = "TestMake",
            Model = "TestModel",
            Generation = "TestGeneration",
            Year = 2024
        };

        await _dbContext.Vehicles.AddAsync(newVehicle);
        await _dbContext.SaveChangesAsync();

        var branchVehicle = new BranchVehicle
        {
            BranchId = branch.Id,
            VehicleId = vehicle.Id,
        };

        await _dbContext.BranchesVehicles.AddAsync(branchVehicle);
        await _dbContext.SaveChangesAsync();

        const string invalidId = "9c0287a9-4bdc-4937-a22b-c9d1c7118803";

        // Act
        var patchDoc = new JsonPatchDocument<UpdateBranchVehicleDto>();
        patchDoc.Replace(c => c.VehicleId, newVehicle.Id);

        var serializedPatchDoc = JsonConvert.SerializeObject(patchDoc);
        var content = new StringContent(serializedPatchDoc, Encoding.UTF8, "application/json");
        var response = await Client.PatchAsync($"api/branchvehicle/{invalidId}", content);

        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
    }

    [Fact]
    public async Task DeleteBranchVehicleAsync_ValidId_ShouldReturnNoContentStatusCode()
    {
        // Arrange
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

        await _dbContext.Companies.AddAsync(company);
        await _dbContext.SaveChangesAsync();

        var branch = new Branch
        {
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

        await _dbContext.Branches.AddAsync(branch);
        await _dbContext.SaveChangesAsync();

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
            Vin = "1C3CDFBBXFD224921",
            Make = "TestMake",
            Model = "TestModel",
            Generation = "TestGeneration",
            Year = 2024
        };

        await _dbContext.Vehicles.AddAsync(vehicle);
        await _dbContext.SaveChangesAsync();

        var branchVehicle = new BranchVehicle
        {
            BranchId = branch.Id,
            VehicleId = vehicle.Id,
        };

        await _dbContext.BranchesVehicles.AddAsync(branchVehicle);
        await _dbContext.SaveChangesAsync();

        // Act
        var response = await Client.DeleteAsync($"api/branchvehicle/{branchVehicle.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task DeleteBranchVehicleAsync_InvalidId_ShouldReturnNoContentStatusCode()
    {
        // Arrange
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

        await _dbContext.Companies.AddAsync(company);
        await _dbContext.SaveChangesAsync();

        var branch = new Branch
        {
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

        await _dbContext.Branches.AddAsync(branch);
        await _dbContext.SaveChangesAsync();

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
            Vin = "1C3CDFBBXFD224921",
            Make = "TestMake",
            Model = "TestModel",
            Generation = "TestGeneration",
            Year = 2024
        };

        await _dbContext.Vehicles.AddAsync(vehicle);
        await _dbContext.SaveChangesAsync();

        var branchVehicle = new BranchVehicle
        {
            BranchId = branch.Id,
            VehicleId = vehicle.Id,
        };

        await _dbContext.BranchesVehicles.AddAsync(branchVehicle);
        await _dbContext.SaveChangesAsync();

        const string invalidId = "9c0287a9-4bdc-4937-a22b-c9d1c7118803";

        // Act
        var response = await Client.DeleteAsync($"api/branchvehicle/{invalidId}");

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