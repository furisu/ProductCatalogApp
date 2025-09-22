using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using ProductCatalogApp.Controllers;
using ProductCatalogApp.Models;
using ProductCatalogApp.Tests.Common;

public class ProductsController_Index_Tests : IDisposable
{
    private readonly ProductCatalogApp.Data.ProductContext _ctx;
    private readonly Microsoft.Data.Sqlite.SqliteConnection _conn;

    public ProductsController_Index_Tests()
    {
        (_ctx, _conn) = TestDb.CreateSqliteContext(); // 空のインメモリDB
        Seed.Load(_ctx);                               // 初期データ投入
    }

    /// <summary>カテゴリID=1で絞り込むと、2件返り、全件のCategoryIdが1であることを確認する。</summary>
    [Fact]
    public async Task Index_CategoryFilter()
    {
        var products = await GetIndexProducts(selectedCategory: 1);

        products.Count.Should().Be(2);　// 件数は2件
        products.All(p => p.CategoryId == 1).Should().BeTrue(); // 全行のCategoryIdが1
    }

    /// <summary>価格昇順を指定すると、カテゴリID=1の2件が価格の小さい順に並ぶことを確認する。</summary>
    [Fact]
    public async Task Index_SortPriceAsc()
    {
        var products = await GetIndexProducts(selectedCategory: 1, sortOrder: "price_asc");

        products.Select(p => p.Price).Should().BeInAscendingOrder(); // 価格が昇順
        products.Select(p => p.Name).Should().Equal("マウス", "ノートPC"); // 名前と順序が一致
    }

    private async Task<List<Product>> GetIndexProducts(
    string? search = null,
    int? selectedCategory = null,
    int? selectedStatus = null,
    decimal? minPrice = null,
    decimal? maxPrice = null,
    int page = 1,
    string sortOrder = "")
    {
        var controller = new ProductsController(_ctx); // テスト対象のコントローラを生成
        var viewResult = await controller.Index(
            searchString: search,
            minPrice: minPrice,
            maxPrice: maxPrice,
            selectedCategory: selectedCategory,
            selectedStatus: selectedStatus,
            page: page,
            sortOrder: sortOrder
        ) as ViewResult;// Indexを直接呼び出し

        return ((IEnumerable<Product>)viewResult!.Model!).ToList(); // ViewのModelを取り出す
    }

    public void Dispose()
    {
        _ctx.Dispose();
        _conn.Dispose();
    }
}



