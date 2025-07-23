using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProductCatalogApp.Data;

var builder = WebApplication.CreateBuilder(args);

// �f�t�H���g�� appsettings.json �̐ڑ�������
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Render ���ϐ� DATABASE_URL ���ݒ肳��Ă���Ώ㏑��
var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
if (!string.IsNullOrEmpty(databaseUrl) && databaseUrl.StartsWith("postgres"))
{
    var uri = new Uri(databaseUrl);
    var userInfo = uri.UserInfo.Split(':');
    connectionString =
        $"Host={uri.Host};Port={uri.Port};Database={uri.AbsolutePath.TrimStart('/')};Username={userInfo[0]};Password={userInfo[1]};SSL Mode=Require;Trust Server Certificate=true";
}

// DbContext�̓o�^�iPostgreSQL�j
builder.Services.AddDbContext<ProductContext>(options =>
    options.UseNpgsql(connectionString ?? throw new InvalidOperationException("�ڑ������񂪌�����܂���B")));

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

