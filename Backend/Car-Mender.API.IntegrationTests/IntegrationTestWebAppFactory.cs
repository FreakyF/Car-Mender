using Car_Mender.Infrastructure.Persistence.DatabaseContext;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Testcontainers.MsSql;

namespace Car_Mender.API.IntegrationTests;

// ReSharper disable once ClassNeverInstantiated.Global
public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
	private readonly MsSqlContainer _dbContainer = new MsSqlBuilder()
		.WithImage("mcr.microsoft.com/mssql/server:2022-latest")
		.WithPassword("Strong_password123!")
		.Build();

	// TODO: https://www.milanjovanovic.tech/blog/testcontainers-integration-testing-using-docker-in-dotnet
	// TODO: https://www.youtube.com/watch?v=tj5ZCtvgXKY
	// TODO: https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-8.0
	// TODO: Setup environment (WebappFActory, BaseIntegratonTest, TestContainers)
	// TODO: Create tests for CompanyController - call API using TestServer's client. Check if you could create company. Expect successfull status code.
	public Task InitializeAsync()
	{
		return _dbContainer.StartAsync();
	}

	public new Task DisposeAsync()
	{
		return _dbContainer.StopAsync();
	}

	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		builder.ConfigureTestServices(services =>
		{
			var descriptorType = typeof(DbContextOptions<AppDbContext>);

			var descriptor = services.SingleOrDefault(s => s.ServiceType == descriptorType);

			if (descriptor is not null) services.Remove(descriptor);

			services.AddDbContext<AppDbContext>(options => options.UseSqlServer(_dbContainer.GetConnectionString()));
		});
	}
}