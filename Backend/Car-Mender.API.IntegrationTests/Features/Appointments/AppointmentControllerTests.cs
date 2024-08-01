using System.Net;
using System.Text;
using Car_Mender.Domain.Enums;
using Car_Mender.Domain.Features.Appointments.DTOs;
using Car_Mender.Domain.Features.Appointments.Entities;
using Car_Mender.Domain.Features.Engines.Entities;
using Car_Mender.Domain.Features.Vehicles.Entities;
using Car_Mender.Infrastructure.Features.Appointments.Commands.CreateAppointment;
using Car_Mender.Infrastructure.Persistence.DatabaseContext;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Car_Mender.API.IntegrationTests.Features.Appointments;

public class AppointmentControllerTests : BaseIntegrationTest, IDisposable
{
    private readonly IServiceScope _scope;
    private readonly IAppDbContext _dbContext;

    public AppointmentControllerTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
        _scope = Server.Services.CreateScope();
        var provider = _scope.ServiceProvider;
        _dbContext = provider.GetRequiredService<IAppDbContext>();
    }

    [Fact]
    public async Task CreateAppointmentAsync_ValidCompany_ShouldReturnSuccessfulStatusCode()
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
            Vin = "1C3CDFBBXFD224921",
            Make = "TestMake",
            Model = "TestModel",
            Generation = "TestGeneration",
            Year = 2024
        };

        await _dbContext.Vehicles.AddAsync(vehicle);
        await _dbContext.SaveChangesAsync();

        var command = new CreateAppointmentCommand
        {
            VehicleId = vehicle.Id,
            Date = DateTime.UtcNow,
            Description = "TestDescription",
            AppointmentStatus = AppointmentStatus.InProgress,
        };

        var json = JsonSerializer.Serialize(command);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await Client.PostAsync("api/appointment", content);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task CreateAppointmentAsync_InvalidAppointment_ShouldReturnBadRequestStatusCode()
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
            Vin = "1C3CDFBBXFD224921",
            Make = "TestMake",
            Model = "TestModel",
            Generation = "TestGeneration",
            Year = 2024
        };

        await _dbContext.Vehicles.AddAsync(vehicle);
        await _dbContext.SaveChangesAsync();

        var command = new CreateAppointmentCommand
        {
            VehicleId = vehicle.Id,
            Date = DateTime.UtcNow.AddYears(-1),
            Description = "TestDescription",
            AppointmentStatus = AppointmentStatus.InProgress,
        };

        var json = JsonSerializer.Serialize(command);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await Client.PostAsync("api/appointment", content);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetAppointmentAsync_ValidId_ShouldReturnValidAppointment()
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
            Vin = "1C3CDFBBXFD224921",
            Make = "TestMake",
            Model = "TestModel",
            Generation = "TestGeneration",
            Year = 2024
        };

        await _dbContext.Vehicles.AddAsync(vehicle);
        await _dbContext.SaveChangesAsync();

        var appointment = new Appointment
        {
            VehicleId = vehicle.Id,
            Date = DateTime.UtcNow.AddYears(-1),
            Description = "TestDescription",
            AppointmentStatus = AppointmentStatus.InProgress,
        };

        await _dbContext.Appointments.AddAsync(appointment);
        await _dbContext.SaveChangesAsync();

        // Act
        var response = await Client.GetAsync($"api/appointment/{appointment.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetCompanyAsync_InvalidId_ShouldReturnNotFoundStatusCode()
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
            Vin = "1C3CDFBBXFD224921",
            Make = "TestMake",
            Model = "TestModel",
            Generation = "TestGeneration",
            Year = 2024
        };

        await _dbContext.Vehicles.AddAsync(vehicle);
        await _dbContext.SaveChangesAsync();

        var appointment = new Appointment
        {
            VehicleId = vehicle.Id,
            Date = DateTime.UtcNow.AddYears(-1),
            Description = "TestDescription",
            AppointmentStatus = AppointmentStatus.InProgress,
        };

        const string invalidId = "9c0287a9-4bdc-4937-a22b-c9d1c7118803";

        await _dbContext.Appointments.AddAsync(appointment);
        await _dbContext.SaveChangesAsync();

        // Act
        var response = await Client.GetAsync($"api/appointment/{invalidId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task UpdateCompanyAsync_ValidId_ShouldReturnSuccessfulStatusCode()
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
            Vin = "1C3CDFBBXFD224921",
            Make = "TestMake",
            Model = "TestModel",
            Generation = "TestGeneration",
            Year = 2024
        };

        await _dbContext.Vehicles.AddAsync(vehicle);
        await _dbContext.SaveChangesAsync();

        var appointment = new Appointment
        {
            VehicleId = vehicle.Id,
            Date = DateTime.UtcNow.AddYears(-1),
            Description = "TestDescription",
            AppointmentStatus = AppointmentStatus.InProgress,
        };

        await _dbContext.Appointments.AddAsync(appointment);
        await _dbContext.SaveChangesAsync();

        var patchDoc = new JsonPatchDocument<UpdateAppointmentDto>();
        patchDoc.Replace(c => c.Description, "NewTestDescription");

        var serializedPatchDoc = JsonConvert.SerializeObject(patchDoc);
        var content = new StringContent(serializedPatchDoc, Encoding.UTF8, "application/json");
        var response = await Client.PatchAsync($"api/appointment/{appointment.Id}", content);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task UpdateCompanyAsync_InvalidId_ShouldReturnSuccessfulStatusCode()
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
            Vin = "1C3CDFBBXFD224921",
            Make = "TestMake",
            Model = "TestModel",
            Generation = "TestGeneration",
            Year = 2024
        };

        await _dbContext.Vehicles.AddAsync(vehicle);
        await _dbContext.SaveChangesAsync();

        var appointment = new Appointment
        {
            VehicleId = vehicle.Id,
            Date = DateTime.UtcNow.AddYears(-1),
            Description = "TestDescription",
            AppointmentStatus = AppointmentStatus.InProgress,
        };

        const string invalidId = "9c0287a9-4bdc-4937-a22b-c9d1c7118803";

        await _dbContext.Appointments.AddAsync(appointment);
        await _dbContext.SaveChangesAsync();

        var patchDoc = new JsonPatchDocument<UpdateAppointmentDto>();
        patchDoc.Replace(c => c.Description, "NewTestDescription");

        var serializedPatchDoc = JsonConvert.SerializeObject(patchDoc);
        var content = new StringContent(serializedPatchDoc, Encoding.UTF8, "application/json");
        var response = await Client.PatchAsync($"api/appointment/{invalidId}", content);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeleteCompanyAsync_ValidId_ShouldReturnNoContentStatusCode()
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
            Vin = "1C3CDFBBXFD224921",
            Make = "TestMake",
            Model = "TestModel",
            Generation = "TestGeneration",
            Year = 2024
        };

        await _dbContext.Vehicles.AddAsync(vehicle);
        await _dbContext.SaveChangesAsync();

        var appointment = new Appointment
        {
            VehicleId = vehicle.Id,
            Date = DateTime.UtcNow.AddYears(-1),
            Description = "TestDescription",
            AppointmentStatus = AppointmentStatus.InProgress,
        };

        await _dbContext.Appointments.AddAsync(appointment);
        await _dbContext.SaveChangesAsync();

        // Act
        var response = await Client.DeleteAsync($"api/appointment/{appointment.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task DeleteCompanyAsync_InvalidId_ShouldReturnNoContentStatusCode()
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
            Vin = "1C3CDFBBXFD224921",
            Make = "TestMake",
            Model = "TestModel",
            Generation = "TestGeneration",
            Year = 2024
        };

        await _dbContext.Vehicles.AddAsync(vehicle);
        await _dbContext.SaveChangesAsync();

        var appointment = new Appointment
        {
            VehicleId = vehicle.Id,
            Date = DateTime.UtcNow.AddYears(-1),
            Description = "TestDescription",
            AppointmentStatus = AppointmentStatus.InProgress,
        };

        const string invalidId = "9c0287a9-4bdc-4937-a22b-c9d1c7118803";

        await _dbContext.Appointments.AddAsync(appointment);
        await _dbContext.SaveChangesAsync();

        // Act
        var response = await Client.DeleteAsync($"api/appointment/{invalidId}");

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