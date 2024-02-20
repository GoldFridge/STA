using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Sibers_test_app.Domain;
using Sibers_test_app.Services;

var builder = WebApplication.CreateBuilder(args);

// Загрузка конфигурации из файла appsettings.json
builder.Configuration.AddJsonFile("appsettings.json");

// Добавление сервиса контекста данных
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddControllersWithViews();

// Добавление сервисов контроллеров
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Конфигурация HTTP request pipeline

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