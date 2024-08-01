using System.Net;
using System.Text;
using Car_Mender.Domain.Enums;
using Car_Mender.Domain.Features.Appointments.DTOs;
using Car_Mender.Domain.Features.Appointments.Entities;
using Car_Mender.Domain.Features.Branches.Entities;
using Car_Mender.Domain.Features.Companies.Entities;
using Car_Mender.Domain.Features.Engines.Entities;
using Car_Mender.Domain.Features.Issues.DTOs;
using Car_Mender.Domain.Features.Issues.Entities;
using Car_Mender.Domain.Features.Vehicles.Entities;
using Car_Mender.Domain.Features.Workers.Entities;
using Car_Mender.Domain.Models;
using Car_Mender.Infrastructure.Features.Appointments.Commands.CreateAppointment;
using Car_Mender.Infrastructure.Features.Issues.Commands.CreateIssue;
using Car_Mender.Infrastructure.Persistence.DatabaseContext;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Car_Mender.API.IntegrationTests.Features.Issues;

public class IssueControllerTests : BaseIntegrationTest, IDisposable
{
    private readonly IServiceScope _scope;
    private readonly IAppDbContext _dbContext;

    public IssueControllerTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
        _scope = Server.Services.CreateScope();
        var provider = _scope.ServiceProvider;
        _dbContext = provider.GetRequiredService<IAppDbContext>();
    }

    [Fact]
    public async Task CreateIssueAsync_ValidCompany_ShouldReturnSuccessfulStatusCode()
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
            Date = DateTime.UtcNow,
            Description = "TestDescription",
            AppointmentStatus = AppointmentStatus.InProgress,
        };

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

        var worker = new Worker
        {
            BranchId = branch.Id,
            Email = "test@mail.com",
            Password = "Password123!",
            FirstName = "TestFirstName",
            LastName = "TestLastName",
            Phone = "123456789"
        };

        await _dbContext.Workers.AddAsync(worker);
        await _dbContext.SaveChangesAsync();

        var command = new CreateIssueCommand
        {
            AppointmentId = appointment.Id,
            CreatorId = worker.Id,
            ReporterType = ReporterType.Customer,
            Description = "TestDescription",
            ReportedDate = DateTime.UtcNow,
            Status = IssueStatus.Reported
        };

        var json = JsonSerializer.Serialize(command);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await Client.PostAsync("api/issue", content);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task CreateIssueAsync_InvalidIssue_ShouldReturnBadRequestStatusCode()
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
            Date = DateTime.UtcNow,
            Description = "TestDescription",
            AppointmentStatus = AppointmentStatus.InProgress,
        };

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

        var worker = new Worker
        {
            BranchId = branch.Id,
            Email = "test@mail.com",
            Password = "Password123!",
            FirstName = "TestFirstName",
            LastName = "TestLastName",
            Phone = "123456789"
        };

        await _dbContext.Workers.AddAsync(worker);
        await _dbContext.SaveChangesAsync();

        var command = new CreateIssueCommand
        {
            AppointmentId = appointment.Id,
            CreatorId = worker.Id,
            ReporterType = ReporterType.Customer,
            Description = "TestDescription",
            ReportedDate = DateTime.UtcNow.AddYears(-1),
            Status = IssueStatus.Reported
        };

        var json = JsonSerializer.Serialize(command);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await Client.PostAsync("api/issue", content);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetIssueAsync_ValidId_ShouldReturnValidIssue()
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
            Date = DateTime.UtcNow,
            Description = "TestDescription",
            AppointmentStatus = AppointmentStatus.InProgress,
        };

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

        var worker = new Worker
        {
            BranchId = branch.Id,
            Email = "test@mail.com",
            Password = "Password123!",
            FirstName = "TestFirstName",
            LastName = "TestLastName",
            Phone = "123456789"
        };

        await _dbContext.Workers.AddAsync(worker);
        await _dbContext.SaveChangesAsync();

        var issue = new Issue
        {
            AppointmentId = appointment.Id,
            CreatorId = worker.Id,
            ReporterType = ReporterType.Customer,
            Description = "TestDescription",
            ReportedDate = DateTime.UtcNow,
            Status = IssueStatus.Reported
        };

        await _dbContext.Issues.AddAsync(issue);
        await _dbContext.SaveChangesAsync();

        // Act
        var response = await Client.GetAsync($"api/issue/{issue.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetIssueAsync_InvalidId_ShouldReturnNotFoundStatusCode()
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
            Date = DateTime.UtcNow,
            Description = "TestDescription",
            AppointmentStatus = AppointmentStatus.InProgress,
        };

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

        var worker = new Worker
        {
            BranchId = branch.Id,
            Email = "test@mail.com",
            Password = "Password123!",
            FirstName = "TestFirstName",
            LastName = "TestLastName",
            Phone = "123456789"
        };

        await _dbContext.Workers.AddAsync(worker);
        await _dbContext.SaveChangesAsync();

        var issue = new Issue
        {
            AppointmentId = appointment.Id,
            CreatorId = worker.Id,
            ReporterType = ReporterType.Customer,
            Description = "TestDescription",
            ReportedDate = DateTime.UtcNow,
            Status = IssueStatus.Reported
        };

        const string invalidId = "9c0287a9-4bdc-4937-a22b-c9d1c7118803";

        await _dbContext.Issues.AddAsync(issue);
        await _dbContext.SaveChangesAsync();

        // Act
        var response = await Client.GetAsync($"api/issue/{invalidId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task UpdateIssueAsync_ValidId_ShouldReturnSuccessfulStatusCode()
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
            Date = DateTime.UtcNow,
            Description = "TestDescription",
            AppointmentStatus = AppointmentStatus.InProgress,
        };

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

        var worker = new Worker
        {
            BranchId = branch.Id,
            Email = "test@mail.com",
            Password = "Password123!",
            FirstName = "TestFirstName",
            LastName = "TestLastName",
            Phone = "123456789"
        };

        await _dbContext.Workers.AddAsync(worker);
        await _dbContext.SaveChangesAsync();

        var issue = new Issue
        {
            AppointmentId = appointment.Id,
            CreatorId = worker.Id,
            ReporterType = ReporterType.Customer,
            Description = "TestDescription",
            ReportedDate = DateTime.UtcNow,
            Status = IssueStatus.Reported
        };

        await _dbContext.Issues.AddAsync(issue);
        await _dbContext.SaveChangesAsync();

        var patchDoc = new JsonPatchDocument<UpdateIssueDto>();
        patchDoc.Replace(c => c.Description, "NewTestDescription");

        var serializedPatchDoc = JsonConvert.SerializeObject(patchDoc);
        var content = new StringContent(serializedPatchDoc, Encoding.UTF8, "application/json");
        var response = await Client.PatchAsync($"api/issue/{issue.Id}", content);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task UpdateIssueAsync_InvalidId_ShouldReturnSuccessfulStatusCode()
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
            Date = DateTime.UtcNow,
            Description = "TestDescription",
            AppointmentStatus = AppointmentStatus.InProgress,
        };

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

        var worker = new Worker
        {
            BranchId = branch.Id,
            Email = "test@mail.com",
            Password = "Password123!",
            FirstName = "TestFirstName",
            LastName = "TestLastName",
            Phone = "123456789"
        };

        await _dbContext.Workers.AddAsync(worker);
        await _dbContext.SaveChangesAsync();

        var issue = new Issue
        {
            AppointmentId = appointment.Id,
            CreatorId = worker.Id,
            ReporterType = ReporterType.Customer,
            Description = "TestDescription",
            ReportedDate = DateTime.UtcNow,
            Status = IssueStatus.Reported
        };

        const string invalidId = "9c0287a9-4bdc-4937-a22b-c9d1c7118803";

        await _dbContext.Issues.AddAsync(issue);
        await _dbContext.SaveChangesAsync();

        var patchDoc = new JsonPatchDocument<UpdateIssueDto>();
        patchDoc.Replace(c => c.Description, "NewTestDescription");

        var serializedPatchDoc = JsonConvert.SerializeObject(patchDoc);
        var content = new StringContent(serializedPatchDoc, Encoding.UTF8, "application/json");
        var response = await Client.PatchAsync($"api/issue/{invalidId}", content);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeleteIssueAsync_ValidId_ShouldReturnNoContentStatusCode()
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
            Date = DateTime.UtcNow,
            Description = "TestDescription",
            AppointmentStatus = AppointmentStatus.InProgress,
        };

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

        var worker = new Worker
        {
            BranchId = branch.Id,
            Email = "test@mail.com",
            Password = "Password123!",
            FirstName = "TestFirstName",
            LastName = "TestLastName",
            Phone = "123456789"
        };

        await _dbContext.Workers.AddAsync(worker);
        await _dbContext.SaveChangesAsync();

        var issue = new Issue
        {
            AppointmentId = appointment.Id,
            CreatorId = worker.Id,
            ReporterType = ReporterType.Customer,
            Description = "TestDescription",
            ReportedDate = DateTime.UtcNow,
            Status = IssueStatus.Reported
        };

        await _dbContext.Issues.AddAsync(issue);
        await _dbContext.SaveChangesAsync();

        // Act
        var response = await Client.DeleteAsync($"api/issue/{issue.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task DeleteIssueAsync_InvalidId_ShouldReturnNoContentStatusCode()
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
            Date = DateTime.UtcNow,
            Description = "TestDescription",
            AppointmentStatus = AppointmentStatus.InProgress,
        };

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

        var worker = new Worker
        {
            BranchId = branch.Id,
            Email = "test@mail.com",
            Password = "Password123!",
            FirstName = "TestFirstName",
            LastName = "TestLastName",
            Phone = "123456789"
        };

        await _dbContext.Workers.AddAsync(worker);
        await _dbContext.SaveChangesAsync();

        var issue = new Issue
        {
            AppointmentId = appointment.Id,
            CreatorId = worker.Id,
            ReporterType = ReporterType.Customer,
            Description = "TestDescription",
            ReportedDate = DateTime.UtcNow,
            Status = IssueStatus.Reported
        };

        const string invalidId = "9c0287a9-4bdc-4937-a22b-c9d1c7118803";

        await _dbContext.Issues.AddAsync(issue);
        await _dbContext.SaveChangesAsync();

        // Act
        var response = await Client.DeleteAsync($"api/issue/{invalidId}");

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