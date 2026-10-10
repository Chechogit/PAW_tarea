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
    protected override IQueryable<Product> BaseQuery()
    {
        return DbSet
            .AsNoTracking()
            .Include(p => p.Category)
            .Include(p => p.Inventory)
            .Include(p => p.Supplier);
    }

    public override async Task<Product?> FindAsync(object id)
    {
        try
        {
            var productId = Convert.ToInt32(id);
            return await BaseQuery()
                .FirstOrDefaultAsync(p => p.ProductId == productId);
        }
        catch (Exception ex)
        {
            throw new PAWException(ex);
        }
    }
}