using PAW.DataAccess.MSSQL;
using PAW.Models;
using PAW.Repositories;

namespace PAW.DataAccess.Repositories;

public interface ISupplierRepository : IRepositoryBase<Supplier>
{
}

public class SupplierRepository(ProductDbContext context)
    : RepositoryBase<Supplier>(context), ISupplierRepository
{
}
