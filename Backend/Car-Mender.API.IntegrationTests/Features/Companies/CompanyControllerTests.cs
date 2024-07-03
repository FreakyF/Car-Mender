using System.Text;
using System.Text.Json;
using Car_Mender.Domain.Models;
using Car_Mender.Infrastructure.Features.Companies.Commands.CreateCompany;

namespace Car_Mender.API.IntegrationTests.Features.Companies;

public class CompanyControllerTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
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
		Assert.True(response.IsSuccessStatusCode);
	}
}