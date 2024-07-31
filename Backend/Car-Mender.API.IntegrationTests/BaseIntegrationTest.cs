using Car_Mender.Infrastructure.Persistence.DatabaseContext;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;

namespace Car_Mender.API.IntegrationTests;

public abstract class BaseIntegrationTest
	: IClassFixture<IntegrationTestWebAppFactory>,
		IDisposable
{
	private readonly AppDbContext _dbContext;
	private readonly IServiceScope _scope;
	protected readonly HttpClient Client;
	protected readonly TestServer Server;
	private bool _disposed;

	protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
	{
		Server = factory.Server;
		_scope = Server.Services.CreateScope();
		Client = Server.CreateClient();
		_dbContext = _scope.ServiceProvider.GetRequiredService<AppDbContext>();
		_dbContext.Database.Migrate();
	}

	public void Dispose()
	{
		_scope.Dispose();
		_dbContext.Dispose();
	}
}