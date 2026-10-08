using Microsoft.EntityFrameworkCore;

namespace InventoryApi;

public class InventoryDbContext : DbContext
{
    public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options) { }

    public DbSet<Item> Items { get; set; }
}