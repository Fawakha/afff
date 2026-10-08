using Microsoft.EntityFrameworkCore;
using InventoryApi;

var builder = WebApplication.CreateBuilder(args);

// Додаємо базові сервіси
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// РЕЄСТРУЄМО CORS (Ось цього рядка не вистачало!)
builder.Services.AddCors();

// Підключаємо наш DbContext до SQL Server
builder.Services.AddDbContext<InventoryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// ВИКОРИСТОВУЄМО CORS (Тепер сервер знає, що це таке)
app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

// Налаштування Swagger 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Створюємо маршрут /inventory
app.MapGet("/inventory", (InventoryDbContext db) =>
{
    return db.Items.ToList(); 
});

app.Run();