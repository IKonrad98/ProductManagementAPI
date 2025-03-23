using ProductManagementAPI.DataAccess.Entity;

public class ProductEntity : BaseEntity

{
    public string Name { get; set; } = default!;
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
}