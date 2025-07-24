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
    var uri = new Uri(databaseUrl);
    var userInfo = uri.UserInfo.Split(':');
    connectionString =
        $"Host={uri.Host};Port={uri.Port};Database={uri.AbsolutePath.TrimStart('/')};Username={userInfo[0]};Password={userInfo[1]};SSL Mode=Require;Trust Server Certificate=true";
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

