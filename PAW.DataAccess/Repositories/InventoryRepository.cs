using PAW.DataAccess.MSSQL;
using PAW.Models;
using PAW.Repositories;

namespace PAW.DataAccess.Repositories;

public interface IInventoryRepository : IRepositoryBase<Inventory>
{
}

public class InventoryRepository(ProductDbContext context)
    : RepositoryBase<Inventory>(context), IInventoryRepository
{
}
