using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProductCatalogApp.Data;

var builder = WebApplication.CreateBuilder(args);

// デフォルトは appsettings.json の接続文字列
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Render 環境変数 DATABASE_URL が設定されていれば上書き
var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
if (!string.IsNullOrEmpty(databaseUrl) && databaseUrl.StartsWith("postgres"))
{
    // 自前でパース
    // 例: postgres://username:password@hostname:port/dbname
    var connectionData = databaseUrl.Replace("postgres://", "");
    var atIndex = connectionData.IndexOf('@');
    var credentials = connectionData.Substring(0, atIndex);
    var hostAndDb = connectionData.Substring(atIndex + 1);

    var userParts = credentials.Split(':');
    var user = userParts[0];
    var password = userParts[1];

    var hostParts = hostAndDb.Split('/');
    var hostPort = hostParts[0];
    var database = hostParts[1];

    var hostPortParts = hostPort.Split(':');
    var host = hostPortParts[0];
    var port = hostPortParts.Length > 1 ? hostPortParts[1] : "5432";

    connectionString =
        $"Host={host};Port={port};Database={database};Username={user};Password={password};SSL Mode=Require;Trust Server Certificate=true";
}


// DbContextの登録（PostgreSQL）
builder.Services.AddDbContext<ProductContext>(options =>
    options.UseNpgsql(connectionString ?? throw new InvalidOperationException("接続文字列が見つかりません。")));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

