using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Tests.Shared.TestUtilities;

public class DatabaseFixture : IDisposable
{
    public ApplicationDbContext Context { get; }
    private readonly string _databaseName;

    public DatabaseFixture()
    {
        _databaseName = $"TestDb_{Guid.NewGuid()}";
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(_databaseName)
            .Options;

        Context = new ApplicationDbContext(options);
    }

    public void Dispose()
    {
        Context.Database.EnsureDeleted();
        Context.Dispose();
    }
}

