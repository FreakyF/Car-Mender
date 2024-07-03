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
	private readonly TestServer _server;
	private bool _disposed;

	protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
	{
		_server = factory.Server;
		_scope = _server.Services.CreateScope();
		Client = _server.CreateClient();
		_dbContext = _scope.ServiceProvider.GetRequiredService<AppDbContext>();
		_dbContext.Database.Migrate();
	}

	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	private void Dispose(bool disposing)
	{
		if (_disposed) return;
		if (disposing)
		{
			_dbContext.Dispose();
			_scope.Dispose();
			Client.Dispose();
			_server.Dispose();
		}

		_disposed = true;
	}
}