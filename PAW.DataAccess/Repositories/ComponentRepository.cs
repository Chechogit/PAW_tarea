using PAW.DataAccess.MSSQL;
using PAW.Models;
using PAW.Repositories;

namespace PAW.DataAccess.Repositories;

public interface IComponentRepository : IRepositoryBase<Component>
{
}

public class ComponentRepository(ProductDbContext context)
    : RepositoryBase<Component>(context), IComponentRepository
{
}
