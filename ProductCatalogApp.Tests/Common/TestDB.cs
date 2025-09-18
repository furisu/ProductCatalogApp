using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ProductCatalogApp.Data;
using Microsoft.Extensions.Logging;




namespace ProductCatalogApp.Tests.Common;

public static class TestDb
{

    public static (ProductContext Ctx, SqliteConnection Conn) CreateSqliteContext()
    {
        var conn = new SqliteConnection("DataSource=:memory:");
        conn.Open();

        var opt = new DbContextOptionsBuilder<ProductContext>()
            .UseSqlite(conn)
            .EnableDetailedErrors()
            .EnableSensitiveDataLogging()
            .LogTo(msg =>
                {
                    if (msg.Contains("sqlite_master") || msg.Contains("CREATE TABLE")) return;
                    if (msg.StartsWith("info:", StringComparison.Ordinal)) Console.WriteLine(msg);
                },
                new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information)
            .Options;

        var ctx = new ProductContext(opt);


        ctx.Database.EnsureCreated();

        return (ctx, conn);
    }
}
