using Evently.Server.Common.Data;
using Microsoft.EntityFrameworkCore;
using Testcontainers.MsSql;

namespace Evently.Server.Test.Common.Setup;

public class DatabaseFixture : IDisposable {
	private readonly MsSqlContainer _container = new MsSqlBuilder().Build();
	private AppDbContext? _dbContext;

	public async Task<AppDbContext> GetDbContext() {
		// if (_container.State == TestcontainersStates.Created) {
		// 	return _dbContext!;
		// }
		await _container.StartAsync();
		string connString = _container.GetConnectionString();
		DbContextOptions<AppDbContext> contextOptions = new DbContextOptionsBuilder<AppDbContext>()
			.UseSqlServer(connString)
			.Options;
		_dbContext = new AppDbContext(contextOptions);
		await _dbContext.Database.EnsureCreatedAsync();
		return _dbContext;
	}

	public void Dispose() {
		_dbContext?.Dispose();
		_container.DisposeAsync();
	}
}
