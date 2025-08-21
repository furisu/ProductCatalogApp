using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ProductCatalogApp.Data;

namespace ProductCatalogApp.Tests.Common;

public static class TestDb
{

    public static (ProductContext Ctx, SqliteConnection Conn) CreateSqliteContext()
    {
        var conn = new SqliteConnection("DataSource=:memory:");
        conn.Open();

        var opt = new DbContextOptionsBuilder<ProductContext>()
            .UseSqlite(conn)
            .Options;

        var ctx = new ProductContext(opt);


        ctx.Database.EnsureCreated();

        return (ctx, conn);
    }
}
