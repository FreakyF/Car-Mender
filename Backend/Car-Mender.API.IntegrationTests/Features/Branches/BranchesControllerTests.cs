using System.Text;
using System.Text.Json;
using Car_Mender.Domain.Features.Companies.Entities;
using Car_Mender.Domain.Models;
using Car_Mender.Infrastructure.Features.Branches.Commands.CreateBranch;
using Car_Mender.Infrastructure.Persistence.DatabaseContext;

namespace Car_Mender.API.IntegrationTests.Features.Branches;

public class BranchesControllerTests : BaseIntegrationTest, IDisposable
{
	private readonly IServiceProvider _services;
	private readonly IServiceScope _scope;

	public BranchesControllerTests(IntegrationTestWebAppFactory factory) : base(factory)
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
	public async Task CreateBranchAsync_ShouldReturnSuccessfulStatusCode()
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

		await dbContext.SaveChangesAsync();
		
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
		Assert.True(response.IsSuccessStatusCode);
	}
}