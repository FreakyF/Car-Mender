using System.Net;
using System.Text;
using Car_Mender.Domain.Features.Branches.Entities;
using Car_Mender.Domain.Features.Companies.Entities;
using Car_Mender.Domain.Features.Workers.DTOs;
using Car_Mender.Domain.Features.Workers.Entities;
using Car_Mender.Domain.Models;
using Car_Mender.Infrastructure.Features.Workers.Commands.CreateWorker;
using Car_Mender.Infrastructure.Persistence.DatabaseContext;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;


namespace Car_Mender.API.IntegrationTests.Features.Workers;

public class WorkerControllerTests : BaseIntegrationTest, IDisposable
{
    private readonly IServiceScope _scope;
    private readonly IAppDbContext _dbContext;

    public WorkerControllerTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
        _scope = Server.Services.CreateScope();
        var provider = _scope.ServiceProvider;
        _dbContext = provider.GetRequiredService<IAppDbContext>();
    }

    [Fact]
    public async Task CreateWorkerAsync_ValidWorker_ShouldReturnSuccessfulStatusCode()
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

        var command = new CreateWorkerCommand
        {
            BranchId = branch.Id,
            Email = "test@mail.com",
            Password = "Password123!",
            FirstName = "TestFirstName",
            LastName = "TestLastName",
            Phone = "123456789"
        };

        var json = JsonSerializer.Serialize(command);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await Client.PostAsync("api/worker", content);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task CreateWorkerAsync_InvalidWorker_ShouldReturnBadRequestStatusCode()
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

        var command = new CreateWorkerCommand
        {
            BranchId = branch.Id,
            Email = "invalid mail",
            Password = "123",
            FirstName = "te",
            LastName = "st",
            Phone = "123"
        };

        var json = JsonSerializer.Serialize(command);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await Client.PostAsync("api/worker", content);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetWorkerAsync_ValidId_ShouldReturnValidWorker()
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

        // Act
        var response = await Client.GetAsync($"api/worker/{worker.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetWorkerAsync_InvalidId_ShouldReturnNotFoundStatusCode()
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

        var worker = new Worker
        {
            BranchId = branch.Id,
            Email = "test@mail.com",
            Password = "Password123!",
            FirstName = "TestFirstName",
            LastName = "TestLastName",
            Phone = "123456789"
        };

        const string invalidId = "9c0287a9-4bdc-4937-a22b-c9d1c7118803";

        await _dbContext.Workers.AddAsync(worker);
        await _dbContext.SaveChangesAsync();

        // Act
        var response = await Client.GetAsync($"api/worker/{invalidId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task UpdateWorkerAsync_ValidId_ShouldReturnNoContentStatusCode()
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


        // Act
        var patchDoc = new JsonPatchDocument<UpdateWorkerDto>();
        patchDoc.Replace(c => c.FirstName, "NewTestFirstName");

        var serializedPatchDoc = JsonConvert.SerializeObject(patchDoc);
        var content = new StringContent(serializedPatchDoc, Encoding.UTF8, "application/json");
        var response = await Client.PatchAsync($"api/worker/{worker.Id}", content);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task UpdateWorkerAsync_InvalidId_ShouldReturnNotFoundStatusCode()
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

        var worker = new Worker
        {
            BranchId = branch.Id,
            Email = "test@mail.com",
            Password = "Password123!",
            FirstName = "TestFirstName",
            LastName = "TestLastName",
            Phone = "123456789"
        };

        const string invalidId = "9c0287a9-4bdc-4937-a22b-c9d1c7118803";

        await _dbContext.Workers.AddAsync(worker);
        await _dbContext.SaveChangesAsync();

        // Act
        var patchDoc = new JsonPatchDocument<UpdateWorkerDto>();
        patchDoc.Replace(c => c.FirstName, "NewTestFirstName");

        var serializedPatchDoc = JsonConvert.SerializeObject(patchDoc);
        var content = new StringContent(serializedPatchDoc, Encoding.UTF8, "application/json");
        var response = await Client.PatchAsync($"api/worker/{invalidId}", content);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeleteWorkerAsync_ValidId_ShouldReturnNotContentStatusCode()
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

        // Act
        var response = await Client.DeleteAsync($"api/worker/{worker.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task DeleteWorkerAsync_InvalidId_ShouldReturnNotFoundStatusCode()
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

        var worker = new Worker
        {
            BranchId = branch.Id,
            Email = "test@mail.com",
            Password = "Password123!",
            FirstName = "TestFirstName",
            LastName = "TestLastName",
            Phone = "123456789"
        };

        const string invalidId = "9c0287a9-4bdc-4937-a22b-c9d1c7118803";

        await _dbContext.Workers.AddAsync(worker);
        await _dbContext.SaveChangesAsync();

        // Act
        var response = await Client.DeleteAsync($"api/worker/{invalidId}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }


    public new void Dispose()
    {
        _scope.Dispose();
        base.Dispose();
    }
}