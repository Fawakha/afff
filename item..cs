namespace InventoryApi;

public class Item
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public decimal Wear { get; set; }
    public decimal Price { get; set; }
}