using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ProductManagementAPI.DataAccess;

public class ProductManagementDb : DbContext
{
    public DbSet<ProductEntity> Products { get; set; } = default!;

    public ProductManagementDb(DbContextOptions<ProductManagementDb> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}