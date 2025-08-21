using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using ProductCatalogApp.Controllers;
using ProductCatalogApp.Models;
using ProductCatalogApp.Tests.Common;

namespace ProductCatalogApp.Tests.Controllers;

public class ProductsController_Index_Tests : IDisposable
{
    private readonly ProductCatalogApp.Data.ProductContext _ctx;
    private readonly Microsoft.Data.Sqlite.SqliteConnection _conn;

    public ProductsController_Index_Tests()
    {
        (_ctx, _conn) = TestDb.CreateSqliteContext();
        Seed.Load(_ctx);
    }

    [Fact]
    public async Task カテゴリ1_価格昇順_ページ1で_期待順序になる()
    {
        var sut = new ProductsController(_ctx);

        var result = await sut.Index(
            searchString: null,
            minPrice: null,
            maxPrice: null,
            selectedCategory: 1,
            selectedStatus: null,
            page: 1,
            sortOrder: "price_asc"
        ) as ViewResult;

        result.Should().NotBeNull();
        var model = result!.Model.Should().BeAssignableTo<IEnumerable<Product>>().Subject.ToList();


        model.Select(x => x.Name).Should().ContainInOrder("マウス", "ノートPC");


        result.ViewData["CurrentPage"].Should().Be(1);
        result.ViewData["TotalPages"].Should().Be(1);
    }

    public void Dispose()
    {
        _ctx.Dispose();
        _conn.Dispose();
    }
}
