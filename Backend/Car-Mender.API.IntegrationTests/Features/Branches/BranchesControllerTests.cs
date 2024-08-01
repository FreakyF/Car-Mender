using System.Net;
using System.Text;
using Car_Mender.Domain.Features.Branches.DTOs;
using Car_Mender.Domain.Features.Branches.Entities;
using Car_Mender.Domain.Features.Companies.Entities;
using Car_Mender.Domain.Models;
using Car_Mender.Infrastructure.Features.Branches.Commands.CreateBranch;
using Car_Mender.Infrastructure.Persistence.DatabaseContext;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Car_Mender.API.IntegrationTests.Features.Branches;

public class BranchesControllerTests : BaseIntegrationTest, IDisposable
{
    private readonly IServiceScope _scope;
    private readonly IAppDbContext _dbContext;

    public BranchesControllerTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
        _scope = Server.Services.CreateScope();
        var provider = _scope.ServiceProvider;
        _dbContext = provider.GetRequiredService<IAppDbContext>();
    }

    [Fact]
    public async Task CreateBranchAsync_ValidBranch_ShouldReturnSuccessfulStatusCode()
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

        var command = new CreateBranchCommand
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

        var json = JsonSerializer.Serialize(command);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await Client.PostAsync("api/branch", content);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task CreateBranchAsync_InvalidBranch_ShouldReturnBadRequestStatusCode()
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

        var command = new CreateBranchCommand
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
            Email = "invalid mail",
            Phone = "123"
        };

        var json = JsonSerializer.Serialize(command);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await Client.PostAsync("api/branch", content);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetBranchAsync_ValidId_ShouldReturnValidBranch()
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

        // Act
        var response = await Client.GetAsync($"api/branch/{branch.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetBranchAsync_InvalidId_ShouldReturnNotFoundStatusCode()
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
            Email = "invalid mail",
            Phone = "123"
        };

        const string invalidId = "9c0287a9-4bdc-4937-a22b-c9d1c7118803";

        await _dbContext.Branches.AddAsync(branch);
        await _dbContext.SaveChangesAsync();

        // Act
        var response = await Client.GetAsync($"api/branch/{invalidId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task UpdateBranchAsync_ValidId_ShouldReturnNoContentStatusCode()
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
            Email = "invalid mail",
            Phone = "123"
        };

        await _dbContext.Branches.AddAsync(branch);
        await _dbContext.SaveChangesAsync();

        // Act
        var patchDoc = new JsonPatchDocument<UpdateBranchDto>();
        patchDoc.Replace(c => c.Name, "NewTestName");

        var serializedPatchDoc = JsonConvert.SerializeObject(patchDoc);
        var content = new StringContent(serializedPatchDoc, Encoding.UTF8, "application/json");
        var response = await Client.PatchAsync($"api/branch/{branch.Id}", content);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task UpdateBranchAsync_InvalidId_ShouldReturnNotFoundStatusCode()
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
            Email = "invalid mail",
            Phone = "123"
        };
        
        const string invalidId = "9c0287a9-4bdc-4937-a22b-c9d1c7118803";

        await _dbContext.Branches.AddAsync(branch);
        await _dbContext.SaveChangesAsync();

        var patchDoc = new JsonPatchDocument<UpdateBranchDto>();
        patchDoc.Replace(c => c.Name, "NewTestName");

        var serializedPatchDoc = JsonConvert.SerializeObject(patchDoc);
        var content = new StringContent(serializedPatchDoc, Encoding.UTF8, "application/json");
        var response = await Client.PatchAsync($"api/branch/{invalidId}", content);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact]
    public async Task DeleteBranchAsync_ValidId_ShouldReturnNoContentStatusCode()
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
            Email = "invalid mail",
            Phone = "123"
        };
        
        await _dbContext.Branches.AddAsync(branch);
        await _dbContext.SaveChangesAsync();

        // Act
        var response = await Client.DeleteAsync($"api/branch/{branch.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
    
    [Fact]
    public async Task DeleteBranchAsync_InvalidId_ShouldReturnNoContentStatusCode()
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
            Email = "invalid mail",
            Phone = "123"
        };
        
        const string invalidId = "9c0287a9-4bdc-4937-a22b-c9d1c7118803";

        await _dbContext.Branches.AddAsync(branch);
        await _dbContext.SaveChangesAsync();

        // Act
        var response = await Client.DeleteAsync($"api/branch/{invalidId}");

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