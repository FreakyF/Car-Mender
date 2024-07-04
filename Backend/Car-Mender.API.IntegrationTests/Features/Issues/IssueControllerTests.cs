using System.Text;
using System.Text.Json;
using Car_Mender.Domain.Enums;
using Car_Mender.Domain.Features.Appointments.Entities;
using Car_Mender.Domain.Features.Branches.Entities;
using Car_Mender.Domain.Features.Companies.Entities;
using Car_Mender.Domain.Features.Engines.Entities;
using Car_Mender.Domain.Features.Vehicles.Entities;
using Car_Mender.Domain.Features.Workers.Entities;
using Car_Mender.Domain.Models;
using Car_Mender.Infrastructure.Features.Issues.Commands.CreateIssue;
using Car_Mender.Infrastructure.Persistence.DatabaseContext;

namespace Car_Mender.API.IntegrationTests.Features.Issues;

public class IssueControllerTests : BaseIntegrationTest, IDisposable
{
	private readonly IServiceProvider _services;
	private readonly IServiceScope _scope;

	public IssueControllerTests(IntegrationTestWebAppFactory factory) : base(factory)
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
	public async Task CreateIssueAsync_ShouldReturnSuccessfulStatusCode()
	{
		// Arrange
		var dbContext = _services.GetRequiredService<AppDbContext>();

		var engine = new Engine
		{
			EngineCode = "TestCode",
			Displacement = 1600,
			PowerKw = 78,
			TorqueNm = 120,
			FuelType = FuelType.Gasoline
		};

		await dbContext.Engines.AddAsync(engine);

		var vehicle = new Vehicle
		{
			Vin = "TestVin1234567890",
			Make = "TestMake",
			Model = "TestModel",
			Generation = "TestGeneration",
			Year = 2024,
			EngineId = engine.Id
		};

		await dbContext.Vehicles.AddAsync(vehicle);

		var appointment = new Appointment
		{
			VehicleId = vehicle.Id,
			Date = DateTime.Today.AddDays(1),
			Description = "TestDescription",
			AppointmentStatus = AppointmentStatus.Scheduled,
		};

		await dbContext.Appointments.AddAsync(appointment);

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

		await dbContext.Branches.AddAsync(branch);

		var worker = new Worker
		{
			Email = "test@mail.com",
			Password = "Strong_password123!",
			FirstName = "FirstName",
			LastName = "LastName",
			Phone = "123456789",
			BranchId = branch.Id
		};

		await dbContext.Workers.AddAsync(worker);

		await dbContext.SaveChangesAsync();

		var command = new CreateIssueCommand
		{
			CreatorId = worker.Id,
			AppointmentId = appointment.Id,
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
		Assert.True(response.IsSuccessStatusCode);
	}
}