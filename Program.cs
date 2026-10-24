using Microsoft.EntityFrameworkCore;
using InventoryApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors();

builder.Services.AddDbContext<InventoryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();


app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/inventory", (InventoryDbContext db) =>
{
    return db.Items.ToList(); 
});
app.MapGet("/inventory/{type}", (InventoryDbContext db, string type) =>
{
if (type == "All")
    {
        return db.Items.ToList();
    }
    else
    {
        return db.Items.Where(items => items.Type == type).ToList();
    }
});
app.MapPost("/inventory", (InventoryDbContext db, Item item) =>
{
    if (string.IsNullOrWhiteSpace(item.Name))
    {
        return Results.BadRequest("Назва не може бути порожньою");
    }

    if (item.Price <= 0)
    {
        return Results.BadRequest("Ціна повинна бути більше нуля");
    }

    if (item.Wear < 0 || item.Wear > 1)
    {
        return Results.BadRequest("Стан повинен бути від 0 до 1");
    }
    
    db.Items.Add(item);
    db.SaveChanges();

    return Results.Ok(item);
});
app.MapDelete("/inventory/{id}", (InventoryDbContext db, int id) =>
{
    var item = db.Items.Find(id); 
    if (item == null)
    {
        return Results.NotFound(" Предмета не існує");
    }
    db.Items.Remove(item); 
    db.SaveChanges();
    return Results.Ok(" Предмет видалено"); 
}); 
app.Run();