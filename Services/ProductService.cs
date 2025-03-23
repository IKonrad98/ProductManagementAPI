using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using ProductManagementAPI.DataAccess;
using ProductManagementAPI.DataAccess.Models;
using ProductManagementAPI.Services.Interfaces;

namespace ProductManagementAPI.Services;

public class ProductService : IProductService
{
    private readonly ProductManagementDb _context;
    private readonly IMapper _mapper;

    public ProductService(ProductManagementDb context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProductModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _context.Products.FindAsync([id], cancellationToken);
        return entity is null ? null : _mapper.Map<ProductModel>(entity);
    }

    public async Task<IEnumerable<ProductModel>> GetAllAsync(CancellationToken cancellationToken)
    {
        var entities = await _context.Products.ToListAsync(cancellationToken);
        return _mapper.Map<IEnumerable<ProductModel>>(entities);
    }

    public async Task<ProductModel> CreateAsync(CreateProductModel model, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<ProductEntity>(model);
        entity.Id = Guid.NewGuid();

        _context.Products.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ProductModel>(entity);
    }

    public async Task<ProductModel?> UpdateAsync(Guid id, JsonPatchDocument<UpdateProductModel> patchModel, CancellationToken cancellationToken)
    {
        var entity = await _context.Products.FindAsync([id], cancellationToken);
        if (entity is null) return null;

        var updateModel = _mapper.Map<UpdateProductModel>(entity);
        patchModel.ApplyTo(updateModel);

        _mapper.Map(updateModel, entity);

        await _context.SaveChangesAsync(cancellationToken);
        return _mapper.Map<ProductModel>(entity);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _context.Products.FindAsync([id], cancellationToken);
        if (entity is null) return;

        _context.Products.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}