using ProductCatalogApp.Data;
using ProductCatalogApp.Models;

namespace ProductCatalogApp.Tests.Common;

public static class Seed
{
    public static void Load(ProductContext ctx)
    {
        if (!ctx.Category.Any())
        {
            ctx.Category.AddRange(
                new Category { Id = 1, Name = "電子機器" },
                new Category { Id = 2, Name = "家具" }
            );
        }

        if (!ctx.Status.Any())
        {
            ctx.Status.AddRange(
                new Status { Id = 1, Name = "在庫あり" },
                new Status { Id = 2, Name = "在庫切れ" }
            );
        }

        if (!ctx.Product.Any())
        {
            var now = DateTime.UtcNow;
            ctx.Product.AddRange(
                new Product { Id = 1, Name = "ノートPC", CategoryId = 1, Price = 120000, Stock = 5, StatusId = 1, CreatedAt = now },
                new Product { Id = 2, Name = "ゲーミングチェア", CategoryId = 2, Price = 30000, Stock = 0, StatusId = 2, CreatedAt = now },
                new Product { Id = 3, Name = "マウス", CategoryId = 1, Price = 2500, Stock = 100, StatusId = 1, CreatedAt = now }
            );
        }

        ctx.SaveChanges();
    }
}

