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
    db.Items.Add(item);
    db.SaveChanges();
    return Results.Ok(item);
});
app.Run();