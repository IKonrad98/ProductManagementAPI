using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using ProductManagementAPI.DataAccess.Models;
using ProductManagementAPI.Services.Interfaces;

namespace ProductManagementAPI.Controllers;

[Route("products")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpPost("create")]
    public async Task<ActionResult> CreateAsync(
        [FromForm] CreateProductModel model,
        CancellationToken cancellationToken)
    {
        var product = await _productService.CreateAsync(model, cancellationToken);
        return Ok(product);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var product = await _productService.GetByIdAsync(id, cancellationToken);
        return product is null ? NotFound() : Ok(product);
    }

    [HttpGet]
    public async Task<ActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        var products = await _productService.GetAllAsync(cancellationToken);
        return Ok(products);
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult> UpdateAsync(
        Guid id,
        [FromBody] JsonPatchDocument<UpdateProductModel> patchModel,
        CancellationToken cancellationToken)
    {
        var updated = await _productService.UpdateAsync(id, patchModel, cancellationToken);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        await _productService.DeleteAsync(id, cancellationToken);
        return Ok();
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadImage(
        IFormFile file,
        [FromServices] IFileStorageService fileStorage,
        CancellationToken cancellationToken)
    {
        var objectName = await fileStorage.UploadFileAsync(file, "products", cancellationToken);
        return Ok(new { objectName });
    }
}