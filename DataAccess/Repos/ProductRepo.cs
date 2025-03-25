using Microsoft.EntityFrameworkCore;
using ProductManagementAPI.DataAccess.Repos.Generic;
using ProductManagementAPI.DataAccess.Repos.RepoInterface;

namespace ProductManagementAPI.DataAccess.Repos;

public class ProductRepo : BaseGenericRepo<ProductEntity>, IProductRepo
{
    private readonly ProductManagementDb _context;

    public ProductRepo(ProductManagementDb context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProductEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Products.ToListAsync(cancellationToken);
    }
}