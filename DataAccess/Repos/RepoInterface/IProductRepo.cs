namespace ProductManagementAPI.DataAccess.Repos.RepoInterface;

public interface IProductRepo : IBaseGenericRepo<ProductEntity>
{
    Task<IEnumerable<ProductEntity>> GetAllAsync(CancellationToken cancellationToken);
}