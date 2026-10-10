using Microsoft.EntityFrameworkCore;
using PAW.DataAccess.MSSQL;
using PAW.Models;
using PAW.Repositories;

namespace PAW.DataAccess.Repositories;

public interface IProductRepository : IRepositoryBase<Product>
{
}

public class ProductRepository(ProductDbContext context)
    : RepositoryBase<Product>(context), IProductRepository
{

    private IQueryable<Product> QueryWithRelations()
    {
        return DbSet
            .AsNoTracking()
            .Include(p => p.Category)
            .Include(p => p.Inventory)
            .Include(p => p.Supplier);
    }

    public override async Task<IEnumerable<Product>> ReadAsync()
    {
        try
        {
            return await QueryWithRelations().ToListAsync();
        }
        catch (Exception ex)
        {
            throw new PAWException(ex);
        }
    }

    public override async Task<Product?> FindAsync(object id)
    {
        try
        {
            var productId = Convert.ToInt32(id);
            return await QueryWithRelations()
                .FirstOrDefaultAsync(p => p.ProductId == productId);
        }
        catch (Exception ex)
        {
            throw new PAWException(ex);
        }
    }
}