﻿namespace ProductManagementAPI.DataAccess.Models;

public class UpdateProductModel
{
    public string? Name { get; set; }
    public decimal? Price { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
}