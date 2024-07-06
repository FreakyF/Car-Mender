using System.Net;
using System.Text;
using Car_Mender.Domain.Features.Companies.DTOs;
using Car_Mender.Domain.Features.Companies.Entities;
using Car_Mender.Domain.Models;
using Car_Mender.Infrastructure.Features.Companies.Commands.CreateCompany;
using Car_Mender.Infrastructure.Persistence.DatabaseContext;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Car_Mender.API.IntegrationTests.Features.Companies;

public class CompanyControllerTests : BaseIntegrationTest, IDisposable
{
	private readonly IServiceScope _scope;
	private readonly IAppDbContext _dbContext;

	public CompanyControllerTests(IntegrationTestWebAppFactory factory) : base(factory)
	{
		_scope = Server.Services.CreateScope();
		var provider = _scope.ServiceProvider;
		_dbContext = provider.GetRequiredService<IAppDbContext>();
	}

	[Fact]
	public async Task CreateCompanyAsync_ShouldReturnSuccessfulStatusCode()
	{
		// Arrange
		var command = new CreateCompanyCommand
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

		var json = JsonSerializer.Serialize(command);
		var content = new StringContent(json, Encoding.UTF8, "application/json");

		// Act
		var response = await Client.PostAsync("api/company", content);

		// Assert
		Assert.Equal(HttpStatusCode.Created, response.StatusCode);
	}

	[Fact]
	public async Task GetCompanyAsync_ShouldReturnValidCompany()
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

		// Act
		var response = await Client.GetAsync($"api/company/{company.Id}");

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
	}

	[Fact]
	public async Task UpdateCompanyAsync_ShouldReturnSuccessfulStatusCode()
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

		var patchDoc = new JsonPatchDocument<UpdateCompanyDto>();
		patchDoc.Replace(c => c.Name, "NewTestName");

		var serializedPatchDoc = JsonConvert.SerializeObject(patchDoc);
		var content = new StringContent(serializedPatchDoc, Encoding.UTF8, "application/json");
		var response = await Client.PatchAsync($"api/company/{company.Id}", content);

		// Assert
		Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
	}

	[Fact]
	public async Task DeleteCompanyAsync_ShouldReturnNoContentStatusCode()
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

		// Act
		var response = await Client.DeleteAsync($"api/company/{company.Id}");

		// Assert
		Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
	}

	public new void Dispose()
	{
		_scope.Dispose();
		_dbContext.Dispose();
		base.Dispose();
	}
}