using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using ProductManagementAPI.DataAccess.Models;
using ProductManagementAPI.DataAccess.Repos.RepoInterface;
using ProductManagementAPI.Services.Interfaces;

namespace ProductManagementAPI.Services;

public class ProductService : IProductService
{
    private readonly IProductRepo _repo;
    private readonly IMapper _mapper;
    private readonly IFileStorageService _fileStorage;

    public ProductService(IProductRepo repo, IMapper mapper, IFileStorageService fileStorage)
    {
        _repo = repo;
        _mapper = mapper;
        _fileStorage = fileStorage;
    }

    public async Task<ProductModel> CreateAsync(CreateProductModel model, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<ProductEntity>(model);

        if (model.Image is not null)
        {
            entity.ImageUrl = await _fileStorage.UploadFileAsync(model.Image, "products", cancellationToken);
        }

        await _repo.CreateAsync(entity, cancellationToken);
        await _repo.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ProductModel>(entity);
    }

    public async Task<ProductModel> GetByIdAsync(
    Guid id,
    CancellationToken cancellationToken)
    {
        var entity = await _repo.GetByIdAsync(id, cancellationToken);
        var result = _mapper.Map<ProductEntity, ProductModel>(entity);

        return result;
    }

    public async Task<IEnumerable<ProductModel>> GetAllAsync(
        CancellationToken cancellationToken)
    {
        var entities = await _repo.GetAllAsync(cancellationToken);
        var result = _mapper.Map<IEnumerable<ProductEntity>, IEnumerable<ProductModel>>(entities);

        return result;
    }

    public async Task<ProductModel> UpdateAsync(
    Guid id,
    JsonPatchDocument<UpdateProductModel> patchModel,
    CancellationToken cancellationToken)
    {
        var entity = await _repo.GetByIdAsync(id, cancellationToken);
        if (entity is null) return null;

        var updateModel = _mapper.Map<UpdateProductModel>(entity);
        patchModel.ApplyTo(updateModel);

        if (updateModel.ImageUrl != entity.ImageUrl)
        {
            if (!string.IsNullOrEmpty(entity.ImageUrl))
            {
                var oldObjectName = Path.GetFileName(new Uri(entity.ImageUrl).AbsolutePath);
                await _fileStorage.DeleteFileAsync(oldObjectName, "products", cancellationToken);
            }

            entity.ImageUrl = updateModel.ImageUrl;
        }

        _mapper.Map(updateModel, entity);

        var updatedEntity = await _repo.UpdateAsync(entity, cancellationToken);
        await _repo.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ProductModel>(updatedEntity);
    }

    public async Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        var entity = await _repo.GetByIdAsync(id, cancellationToken);

        if (!string.IsNullOrEmpty(entity.ImageUrl))
        {
            var objectName = Path.GetFileName(new Uri(entity.ImageUrl).AbsolutePath);
            await _fileStorage.DeleteFileAsync(objectName, "products", cancellationToken);
        }

        await _repo.DeleteAsync(id, cancellationToken);
        await _repo.SaveChangesAsync(cancellationToken);
    }
}