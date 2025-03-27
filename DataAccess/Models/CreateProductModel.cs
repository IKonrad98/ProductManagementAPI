namespace ProductManagementAPI.DataAccess.Models;

public class CreateProductModel
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public IFormFile? Image { get; set; }
}