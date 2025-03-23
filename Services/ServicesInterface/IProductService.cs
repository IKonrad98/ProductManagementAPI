using Microsoft.AspNetCore.JsonPatch;
using ProductManagementAPI.DataAccess.Models;

namespace ProductManagementAPI.Services.Interfaces;

public interface IProductService
{
    Task<ProductModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IEnumerable<ProductModel>> GetAllAsync(CancellationToken cancellationToken);

    Task<ProductModel> CreateAsync(CreateProductModel model, CancellationToken cancellationToken);

    Task<ProductModel?> UpdateAsync(Guid id, JsonPatchDocument<UpdateProductModel> patchModel, CancellationToken cancellationToken);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}