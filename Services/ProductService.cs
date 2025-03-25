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

    public ProductService(IProductRepo repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<ProductModel> CreateAsync(
        CreateProductModel model,
        CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<CreateProductModel, ProductEntity>(model);
        var addedEntity = await _repo.CreateAsync(entity, cancellationToken);

        await _repo.SaveChangesAsync(cancellationToken);
        var result = _mapper.Map<ProductEntity, ProductModel>(addedEntity);

        return result;
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

        var updateModel = _mapper.Map<ProductEntity, UpdateProductModel>(entity);
        patchModel.ApplyTo(updateModel);
        _mapper.Map(updateModel, entity);

        var updatedEntity = await _repo.UpdateAsync(entity, cancellationToken);
        await _repo.SaveChangesAsync(cancellationToken);

        var result = _mapper.Map<ProductEntity, ProductModel>(updatedEntity);

        return result;
    }

    public async Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        var entity = await _repo.GetByIdAsync(id, cancellationToken);
        await _repo.DeleteAsync(id, cancellationToken);
        await _repo.SaveChangesAsync(cancellationToken);
    }
}